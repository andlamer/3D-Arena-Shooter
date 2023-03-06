using System;
using System.Linq;
using ShooterBase.ScriptableObjects;
using UnityEngine;

namespace ShooterBase.Core
{
    [RequireComponent(typeof(SphereCollider))]
    public class AttackContactArea : MonoBehaviour
    {
        [SerializeField] private string[] targetTags = {"Player"};
        [SerializeField] private AttackStats attackStats;

        public event Action PlayerDamaged;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!targetTags.Contains(other.tag) || !other.TryGetComponent<IStatsDamageable>(out var damageable)) return;
            
            damageable.TakeDamage(attackStats.DamageToAffectedStats);
            PlayerDamaged?.Invoke();
        }
    }
}