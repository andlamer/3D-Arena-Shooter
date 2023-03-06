using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShooterBase.ScriptableObjects;
using ShooterBase.Services;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace ShooterBase.Core
{
    public class BossEnemy : MonoBehaviour, IEnemy
    {
        [SerializeField] private Transform bodyTransform;
        [SerializeField] private BossEnemyStats bossEnemyStats;
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private DamageablePartComponent damageablePartComponent;
        [SerializeField] private ShootingArea shootingArea;

        public event Action<IEnemy> OnEnemyDead;
        
        private ICharacterStatsService _characterStatsService;
        private CancellationTokenSource _cancellationTokenSource;
        private IScoreService _scoreService;
        private Pool _pool;
        
        private Transform _cachedCharacterTransform;
        private bool _chasingEnabled;

        private bool _wasInitialized;

        [Inject]
        private void Construct(ICharacterStatsService characterStatsService,Character character, IScoreService scoreService)
        {
            _characterStatsService = characterStatsService;
            _cachedCharacterTransform = character.transform;
            _scoreService = scoreService;
        }
        
        private void Awake()
        {
            navMeshAgent.acceleration = bossEnemyStats.Acceleration;
            navMeshAgent.speed = bossEnemyStats.SteeringAISpeed;
            navMeshAgent.stoppingDistance = bossEnemyStats.StoppingDistance;
            shootingArea.SetDetectionArea(bossEnemyStats.ShootingArea);
        }

        private void OnEnable()
        {
            if (!_wasInitialized) //Crutch for Zenject pools 
                return;
            
            _cancellationTokenSource = new CancellationTokenSource();
            navMeshAgent.enabled = false;

            if (_cachedCharacterTransform != null)
                EnableAI().Forget();

            damageablePartComponent.MinHealthReached += OnDeath;
        }

        private void OnDisable()
        {
            if (!_wasInitialized)
            {
                _wasInitialized = true;
                return;
            }
            
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
            
            _chasingEnabled = false;

            damageablePartComponent.MinHealthReached -= OnDeath;
        }
        

        private async UniTask EnableAI()
        {
            await UniTask.Yield();

            bodyTransform.LookAt(_cachedCharacterTransform);
            navMeshAgent.enabled = true;
            _chasingEnabled = true;
            
            while (_chasingEnabled)
            {
                if (!navMeshAgent.SetDestination(_cachedCharacterTransform.position))
                    Debug.Log("Player left nav mesh");
                await UniTask.Delay(TimeSpan.FromSeconds(bossEnemyStats.TargetAIUpdateTime), cancellationToken: _cancellationTokenSource.Token);
            }
        }

        private void OnDeath()
        {
            damageablePartComponent.MinHealthReached -= OnDeath;
            _scoreService.IncreaseKilledEnemiesCounter();
            _characterStatsService.IncreaseStat(CreatureStats.Strength, bossEnemyStats.OnKillCharacterStrengthRecovery);

            if (_pool == null)
            {
                gameObject.SetActive(false);
                return;
            }

            OnEnemyDead?.Invoke(this);
            _pool.Despawn(this);
        }

        public void Kill() => OnDeath();
        
        private void SetPool(Pool pool) => _pool = pool;

        public class Pool : MonoMemoryPool<Vector3, BossEnemy>
        {
            protected override void Reinitialize(Vector3 position, BossEnemy item)
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