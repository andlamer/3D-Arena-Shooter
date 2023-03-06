using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using ShooterBase.ScriptableObjects;
using ShooterBase.Services;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace ShooterBase.Core
{
    public class DivingEnemy : MonoBehaviour, IEnemy
    {
        [SerializeField] private Transform bodyTransform;
        [SerializeField] private Transform visualTransform;
        [SerializeField] private AnimationCurve jumpCurve;
        [SerializeField] private DivingEnemyStats divingEnemyStats;
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private DamageablePartComponent damageablePartComponent;
        [SerializeField] private AttackContactArea attackContactArea;
        [SerializeField] private Vector3 bodyLocalOffset = new(0, 1, 0);

        public event Action<IEnemy> OnEnemyDead;

        private ICharacterStatsService _characterStatsService;
        private Transform _cachedCharacterTransform;
        private IScoreService _scoreService;
        
        private Pool _pool;

        private CancellationTokenSource _cancellationTokenSource;
        private Sequence _sequence;
        private Tweener _loweringTweener;
        
        private bool _initialJumpIsFinished;
        private bool _chasingEnabled;
        private bool _aiEnabled;
        private bool _poolWasInitialized;

        [Inject]
        private void Construct(Character character, ICharacterStatsService characterStatsService, IScoreService scoreService)
        {
            _characterStatsService = characterStatsService;
            _scoreService = scoreService;

            if (character != null)
                _cachedCharacterTransform = character.transform;
        }

        private void Update()
        {
            if (!_initialJumpIsFinished) return;

            Turn();
        }

        private void Awake()
        {
            navMeshAgent.acceleration = divingEnemyStats.Acceleration;
            navMeshAgent.speed = divingEnemyStats.SteeringAISpeed;
        }

        private void OnEnable()
        {
            if (!_poolWasInitialized) //Crutch for Zenject pools, since they enabling and disabling objects on start (that's  not preferable behaviour)
                return;

            _cancellationTokenSource = new CancellationTokenSource();
            _initialJumpIsFinished = false;
            _chasingEnabled = false;
            navMeshAgent.enabled = false;

            bodyTransform.localPosition = bodyLocalOffset;
            bodyTransform.localRotation = Quaternion.identity;
            visualTransform.localRotation = Quaternion.identity;

            if (_cachedCharacterTransform != null)
            {
                transform.LookAt(_cachedCharacterTransform.position);
                EnableAI().Forget();
            }

            damageablePartComponent.MinHealthReached += OnDeathFromBullet;
            attackContactArea.PlayerDamaged += OnDeathFromSelfDestruction;
        }

        private void OnDisable()
        {
            if (!_poolWasInitialized) //Crutch for Zenject pools
            {
                _poolWasInitialized = true;
                return;
            }

            _sequence?.Kill();
            _loweringTweener?.Kill();
            _loweringTweener = null;
            
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;

            _aiEnabled = false;
            _chasingEnabled = false;

            damageablePartComponent.MinHealthReached -= OnDeathFromBullet;
            attackContactArea.PlayerDamaged -= OnDeathFromSelfDestruction;
        }
        
        private async UniTask EnableAI()
        {
            await UniTask.Yield();
            
            transform.LookAt(_cachedCharacterTransform.position);
            
            _aiEnabled = true;
            StartAnimationSequence();

            while (_aiEnabled)
            {
                if (_chasingEnabled)
                {
                    if (navMeshAgent.isOnNavMesh)
                    {
                        if (!navMeshAgent.SetDestination(_cachedCharacterTransform.position))
                        {
                            Debug.Log("Player left nav mesh");
                        }
                        else
                        {
                            if (_loweringTweener == null)
                            {
                                RunLoweringTweener();
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("NavMeshAgent isn't placed on nav mesh");
                    }
                }

                await UniTask.Delay(TimeSpan.FromSeconds(divingEnemyStats.TargetAIUpdateTime), cancellationToken: _cancellationTokenSource.Token);
            }
        }

        private void StartAnimationSequence()
        {
            _sequence = DOTween.Sequence();
            var moveTweener = bodyTransform
                .DOLocalMoveY(bodyTransform.localPosition.y + divingEnemyStats.JumpHeight, divingEnemyStats.JumpDuration)
                .SetEase(jumpCurve);
            _sequence.Append(moveTweener);
            
            var rotationTweener = visualTransform
                .DOLocalRotate(new Vector3(90, 0, 0), divingEnemyStats.JumpRotateDuration)
                .OnComplete(() =>
                {
                    _initialJumpIsFinished = true;
                    _chasingEnabled = true;
                    navMeshAgent.enabled = true;
                });
            _sequence.Append(rotationTweener);
            
            _sequence.Play();
        }

        private void RunLoweringTweener()
        {
            if (bodyTransform == null || _cachedCharacterTransform == null) return;

            var flyTime = Vector3.Distance(transform.position, _cachedCharacterTransform.position) * divingEnemyStats.LoweringMultiplier;
            _loweringTweener = bodyTransform.DOLocalMoveY(divingEnemyStats.LoweringMinHeight, flyTime);
        }
        
        private void Turn()
        {
            var rotation = Quaternion.LookRotation(_cachedCharacterTransform.position - bodyTransform.position);
            bodyTransform.rotation = Quaternion.Slerp(bodyTransform.rotation, rotation, divingEnemyStats.LookSpeed * Time.deltaTime);
        }

        private void OnDeathFromBullet() => OnDeath(DeathReason.DirectDamage);

        private void OnDeathFromSelfDestruction() => OnDeath(DeathReason.SelfDestruction);

        private void OnDeath(DeathReason deathReason)
        {
            damageablePartComponent.MinHealthReached -= OnDeathFromBullet;
            attackContactArea.PlayerDamaged -= OnDeathFromSelfDestruction;

            if (deathReason is DeathReason.DirectDamage or DeathReason.RicochetDamage)
            {
                _scoreService.IncreaseKilledEnemiesCounter();
                _characterStatsService.IncreaseStat(CreatureStats.Strength, divingEnemyStats.OnKillCharacterStrengthRecovery);
            }

            OnEnemyDead?.Invoke(this);
            _pool.Despawn(this);
        }

        private void SetPool(Pool pool) => _pool = pool;

        public void Kill() => OnDeath(DeathReason.UltimateAbility);

        public class Pool : MonoMemoryPool<Vector3, DivingEnemy>
        {
            protected override void Reinitialize(Vector3 position, DivingEnemy item)
            {
                var transform = item.transform;
                transform.position = position;
                transform.rotation = Quaternion.identity;
                transform.SetParent(null);
                item.SetPool(this);
            }
        }
    }
}