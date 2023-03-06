using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ShooterBase.Core
{
    [RequireComponent(typeof(SphereCollider))]
    public class ShootingArea : MonoBehaviour
    {
        [SerializeField] private SphereCollider detectionCollider;
        [SerializeField] private WeaponWithHomingBullets weapon;
        [SerializeField] private string[] enemyTags = {"Player"};

        private const int MaxColliders = 15;
        
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isLockedOnTarget;
        private Collider _lastCachedEntry;

        public void SetDetectionArea(float radius)
        {
            detectionCollider.radius = radius;
            DetectEnemiesOnSpawn();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isLockedOnTarget || !enemyTags.Contains(other.tag)) return;
            
            _isLockedOnTarget = true;
            StartShooting(other.transform).Forget();
        }

        private void OnTriggerExit(Collider other)
        {
            if (enemyTags.Contains(other.tag) && other == _lastCachedEntry)
                _isLockedOnTarget = false;
        }

        private async UniTask StartShooting(Transform otherTransform)
        {
            while (_isLockedOnTarget)
            {
                weapon.Shoot(otherTransform);
                await UniTask.Delay(TimeSpan.FromSeconds(weapon.Cooldown), cancellationToken: _cancellationTokenSource.Token);
            }
        }

        private void OnEnable()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        private void DetectEnemiesOnSpawn()
        {
            if (_isLockedOnTarget)
                return;

            var hitColliders = new Collider[MaxColliders];
            Physics.OverlapSphereNonAlloc(detectionCollider.center, detectionCollider.radius, hitColliders);
            
            foreach (var hitCollider in hitColliders.Where(x => x != null))
            {
                if (!enemyTags.Contains(hitCollider.tag)) continue;
                
                _isLockedOnTarget = true;
                _lastCachedEntry = hitCollider;
                StartShooting(hitCollider.transform).Forget();
                break;
            }   
        }

        private void OnDisable()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
            _isLockedOnTarget = false;
        }
    }
}