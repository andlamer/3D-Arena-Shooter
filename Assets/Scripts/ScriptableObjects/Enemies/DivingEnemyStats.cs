using UnityEngine;
using UnityEngine.Serialization;

namespace ShooterBase.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Enemies/Create DivingEnemy", fileName = "DivingEnemy", order = 1)]
    public class DivingEnemyStats : EnemyStats
    {
        [Header("Jump")]
        [SerializeField] private float lookSpeed = 1f;
        [SerializeField] private float jumpHeight = 1f;
        [SerializeField] private float jumpDuration = 1f;
        [SerializeField] private float jumpRotateDuration = 0.5f;

        [Header("Lowering")]
        [SerializeField] private float loweringMultiplier = 2f;
        [SerializeField] private float loweringMinHeight = 0.05f;

        public float LookSpeed => lookSpeed;
        public float JumpHeight => jumpHeight;
        public float JumpDuration => jumpDuration;
        public float JumpRotateDuration => jumpRotateDuration;
        public float LoweringMultiplier => loweringMultiplier;
        public float LoweringMinHeight => loweringMinHeight;
    }
}