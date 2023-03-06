using UnityEngine;

namespace ShooterBase.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Bullets/Create HomingBulletStats", fileName = "HomingBulletStats", order = 1)]
    public class HomingBulletStats : BaseBulletStats
    {
        [SerializeField] private float rotationSpeed;

        public float RotationSpeed => rotationSpeed;
    }
}