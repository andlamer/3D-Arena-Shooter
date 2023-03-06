using ShooterBase.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace ShooterBase.Core
{
    public class WeaponWithHomingBullets : MonoBehaviour
    {
        [SerializeField] private Transform shootingPoint;
        [SerializeField] private WeaponStats weaponStats;

        public float Cooldown => weaponStats.WeaponCooldown;

        private HomingBullet.Pool _simpleBulletPool;

        [Inject]
        private void Construct(HomingBullet.Pool bulletPool)
        {
            _simpleBulletPool = bulletPool;
        }

        public void Shoot(Transform targetTransform)
        {
            var bullet = _simpleBulletPool.Spawn();
            var bulletTransform = bullet.transform;
            bulletTransform.position = shootingPoint.position;
            bulletTransform.rotation = shootingPoint.rotation;
            bulletTransform.SetParent(null);
            bullet.Shoot(targetTransform);
        }
    }
}