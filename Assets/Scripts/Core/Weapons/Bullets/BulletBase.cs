using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShooterBase.ScriptableObjects;
using UnityEngine;

namespace ShooterBase.Core
{
    public class BulletBase : MonoBehaviour
    {
        [SerializeField] private BaseBulletStats bulletStats;

        private CancellationTokenSource _cancellationTokenSource;
        protected BaseBulletStats BaseStats => bulletStats;

        protected virtual void OnEnable()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            
            if (bulletStats.UseAutoDestroy)
                StartAutoDestroyCountDown().Forget();
        }
        
        protected virtual void OnDisable()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        private async UniTask StartAutoDestroyCountDown()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(bulletStats.AutoDestroyTime), cancellationToken: _cancellationTokenSource.Token);
            SelfDestroy();
        }

        private void OnTriggerEnter(Collider other) => OnHitBehaviour(other);

        protected virtual void OnHitBehaviour(Collider otherCollider)
        {
            if (bulletStats.HitTags.Contains(otherCollider.tag) && otherCollider.TryGetComponent<IStatsDamageable>(out var damageable))
            {
                damageable.TakeDamage(bulletStats.DamageToAffectedStats);
                SelfDestroy();
            }

            if (bulletStats.SelfDestroyTags.Contains(otherCollider.tag))
                SelfDestroy();
        }
        
        protected virtual void SelfDestroy()
        {
            gameObject.SetActive(false);
        }
    }
}