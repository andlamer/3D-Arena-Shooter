using DG.Tweening;
using ShooterBase.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace ShooterBase.Core
{
    [RequireComponent(typeof(Rigidbody))]
    public class HomingBullet : BulletBase
    {
        [SerializeField] private Rigidbody bulletRigidBody;
        [SerializeField] private Transform bulletTransform;

        private IMemoryPool _pool; 
        private Transform _targetTransform;
        private HomingBulletStats _bulletStats;

        private bool _shootingEnabled;

        protected override void OnEnable()
        {
            base.OnEnable();
            bulletRigidBody.velocity = Vector3.zero;
        }

        private void Awake()
        {
            _bulletStats = BaseStats as HomingBulletStats;
            _shootingEnabled = _bulletStats != null;
        }

        private void Update()
        {
            if (_shootingEnabled && _targetTransform != null)
                MoveToTarget();
        }

        private void MoveToTarget()
        {
            var targetDirection = _targetTransform.position - transform.position;
 
            var newDirection = Vector3.RotateTowards(bulletTransform.forward, targetDirection,
                _bulletStats.RotationSpeed * Time.deltaTime, 0.0F);
 
            transform.Translate(Vector3.forward * Time.deltaTime * _bulletStats.FlySpeed, Space.Self);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
        
        private void ChaseTarget()
        {
            Debug.Log(_targetTransform.position);
            var forward = bulletTransform.forward;
            var cross = Vector3.Cross(forward, _targetTransform.position - bulletRigidBody.position.normalized);
            bulletRigidBody.velocity = forward * _bulletStats.FlySpeed;
            bulletRigidBody.angularVelocity = cross * _bulletStats.RotationSpeed;
        }

        public void Shoot(Transform targetTransform)
        {
            _targetTransform = targetTransform;
        }

        protected override void SelfDestroy()
        {
            base.SelfDestroy();
            _pool.Despawn(this);
        }

        private void SetPool(IMemoryPool pool) => _pool = pool;
        
        public class Pool : MonoMemoryPool<HomingBullet>
        {
            protected override void Reinitialize(HomingBullet item)
            {
                item.SetPool(this);
            }
        }
    }
}