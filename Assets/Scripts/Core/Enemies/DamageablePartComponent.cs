using System;
using System.Collections.Generic;
using ShooterBase.ScriptableObjects;
using UnityEngine;

namespace ShooterBase.Core
{
    [RequireComponent(typeof(Collider))]
    public class DamageablePartComponent : MonoBehaviour, IStatsDamageable
    {
        [SerializeField] private EnemyStats enemyStats;

        public event Action MinHealthReached;
        
        private Dictionary<CreatureStats, CharacterStat> _enemyStatsDictionary;

        private void Awake()
        {
            _enemyStatsDictionary = new Dictionary<CreatureStats, CharacterStat>
            {
                {
                    CreatureStats.Health,
                    new CharacterStat(enemyStats.MaxHealthPoints, enemyStats.StartingHealthPoints, enemyStats.MinHealthPoints)
                }
            };
        }

        private void OnEnable()
        {
            _enemyStatsDictionary[CreatureStats.Health].MinStatReached += OnMinHealthReached;
            _enemyStatsDictionary[CreatureStats.Health].Reset();
        }

        private void OnMinHealthReached() => MinHealthReached?.Invoke();

        private void OnDisable()
        {
            _enemyStatsDictionary[CreatureStats.Health].MinStatReached -= OnMinHealthReached;
        }

        public void TakeDamage(IEnumerable<DamageToAffectedStat> damage)
        {
            foreach (var stat in damage)
                if (_enemyStatsDictionary.ContainsKey(stat.AffectedCreatureStat))
                    _enemyStatsDictionary[stat.AffectedCreatureStat].DecreaseStat(stat.Damage);
        }

        public bool IsDead() => _enemyStatsDictionary[CreatureStats.Health].GetStatPercent() == 0;
    }
}