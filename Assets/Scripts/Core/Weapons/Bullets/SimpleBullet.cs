using System;
using System.Linq;
using ShooterBase.ScriptableObjects;
using ShooterBase.Services;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace ShooterBase.Core
{
    [RequireComponent(typeof(Rigidbody))]
    public class SimpleBullet : BulletBase
    {
        [SerializeField] private Rigidbody bulletRigidBody;

        private readonly Random _randomGenerator = new();

        private const int MaxColliders = 20;

        private ICharacterStatsService _characterStatsService;
        private CharacterBulletStats _characterBulletStats;
        private IMemoryPool _pool;

        private bool _effectWasTriggered;
        private bool _ricocheted;

        [Inject]
        private void Construct(ICharacterStatsService characterStatsService)
        {
            _characterStatsService = characterStatsService;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            ResetRigidbody();
            _effectWasTriggered = false;
            _ricocheted = false;
        }

        private void Awake()
        {
            _characterBulletStats = BaseStats as CharacterBulletStats;
        }

        public void Shoot(Vector3 direction) => bulletRigidBody.AddForce(direction * BaseStats.FlySpeed, ForceMode.VelocityChange);

        protected override void SelfDestroy()
        {
            base.SelfDestroy();
            _pool.Despawn(this);
        }

        protected override void OnHitBehaviour(Collider otherCollider)
        {
            if (BaseStats.HitTags.Contains(otherCollider.tag) && otherCollider.TryGetComponent<IStatsDamageable>(out var damageable))
            {
                damageable.TakeDamage(BaseStats.DamageToAffectedStats);
                
                if (_characterBulletStats != null && !_effectWasTriggered)
                {
                    if (_randomGenerator.Next(100) < GetEffectChance())
                    {
                        _ricocheted = false;

                        if (_randomGenerator.Next(100) < 50)
                        {
                            var hitColliders = new Collider[MaxColliders];
                            var bulletTransform = bulletRigidBody.transform;
                            var minDistance = float.MaxValue;
                            Collider closestTarget = null;
                            
                            Physics.OverlapSphereNonAlloc(bulletTransform.position, _characterBulletStats.RicochetRadius, hitColliders); //quite expensive operation

                            foreach (var hitCollider in hitColliders.Where(x => x != null
                                                                                && _characterBulletStats.HitTags.Contains(x.tag) 
                                                                                && x.gameObject != otherCollider.gameObject))
                            {
                                var distanceToTarget = (hitCollider.transform.position - bulletTransform.position).magnitude;
                                
                                if (!(distanceToTarget < minDistance)) continue;
                                
                                minDistance = distanceToTarget;
                                closestTarget = hitCollider;
                            }

                            if (closestTarget != null)
                            {
                                ResetRigidbody();
                                Shoot((closestTarget.transform.position - bulletTransform.position).normalized); //This ricochet type is not as reliable as using OnHitScan with Raycasts
                                _ricocheted = true;
                            }
                        }

                        _effectWasTriggered = true;
                    }
                    else
                    {
                        SelfDestroy();
                    }
                }
                else
                {
                    if (_ricocheted && damageable.IsDead()) //this logic should be moved to service or at least to enemies
                    {
                        if (_characterStatsService.GetStatPercent(CreatureStats.Health) < _characterBulletStats.RicochetHpRestoreThreshold)
                        {
                            _characterStatsService.SetToPercent(CreatureStats.Health, _characterBulletStats.RicochetHpRestoreThreshold);
                        }
                        else
                        {
                            _characterStatsService.IncreaseStat(CreatureStats.Strength, _characterBulletStats.StrengthRestore);
                        }
                    }

                    SelfDestroy();
                }
            }

            if (BaseStats.SelfDestroyTags.Contains(otherCollider.tag))
            {
                SelfDestroy();
            }

            float GetEffectChance()
            {
                var currentHpPercent = Math.Clamp(_characterStatsService.GetStatPercent(CreatureStats.Health), 
                    _characterBulletStats.HpPercentageThresholdForMaxChance, 100);
                var currentEffectProgress = 100 - (currentHpPercent - _characterBulletStats.HpPercentageThresholdForMaxChance) /
                    (100 - _characterBulletStats.HpPercentageThresholdForMaxChance) * 100;
                var currentChance = _characterBulletStats.BaseEffectChance + currentEffectProgress * 
                    (_characterBulletStats.MaxEffectChance - _characterBulletStats.BaseEffectChance) / 100;

                return currentChance;
            }
        }
        
        private void ResetRigidbody()
        {
            bulletRigidBody.angularVelocity = Vector3.zero;
            bulletRigidBody.velocity = Vector3.zero;
        }

        private void SetPool(IMemoryPool pool) => _pool = pool;

        public class Pool : MonoMemoryPool<SimpleBullet>
        {
            protected override void Reinitialize(SimpleBullet item)
            {
                item.SetPool(this);
            }
        }
    }
}