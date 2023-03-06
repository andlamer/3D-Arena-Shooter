using UnityEngine;

namespace ShooterBase.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Enemies/Create BossEnemyStats", fileName = "BossEnemyStats", order = 1)]
    public class BossEnemyStats : EnemyStats
    {
        [Header("Shooting")]
        [SerializeField] private float shootingArea = 5;
        
        [Header("AI")]
        [SerializeField] private float stoppingDistance = 2;

        public float ShootingArea => shootingArea;
        public float StoppingDistance => stoppingDistance;
    }
}