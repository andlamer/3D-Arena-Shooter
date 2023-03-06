using UnityEngine;
using Zenject;

namespace ShooterBase.Core
{
    public class BasicWeapon : MonoBehaviour
    {
        [SerializeField] private Transform shootingPoint;

        private SimpleBullet.Pool _simpleBulletPool;

        [Inject]
        private void Construct(SimpleBullet.Pool bulletPool)
        {
            _simpleBulletPool = bulletPool;
        }

        public void Shoot()
        {
            var bullet = _simpleBulletPool.Spawn();
            var bulletTransform = bullet.transform;
            bulletTransform.position = shootingPoint.position;
            bulletTransform.rotation = shootingPoint.rotation;
            bulletTransform.SetParent(null);
            bullet.Shoot(shootingPoint.forward);
        }
    }
}