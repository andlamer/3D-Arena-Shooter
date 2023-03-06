using UnityEngine;
using ScriptableObject = Zenject.ScriptableObject;

namespace ShooterBase.ScriptableObjects
{
    [CreateAssetMenu(menuName = "SpawnSettings/Create SpawnSettings", fileName = "SpawnSettings", order = 1)]
    public class EnemySpawnSettings : ScriptableObject, IEnemySpawnSettings
    {
        [SerializeField] private float minRespawnTime = 2;
        [SerializeField] private float maxRespawnTime = 5;
        [SerializeField] private float divingToBossRatio = 4;
        [SerializeField] private float respawnTimeDecreasePerEnemy = 0.2f;
        [SerializeField] private float respawnAmountIncreasePerEnemy = 0.05f;


        public float MinRespawnTime => minRespawnTime;
        public float MaxRespawnTime => maxRespawnTime;
        public float DivingToBossRatio => divingToBossRatio;
        public float RespawnTimeDecreasePerEnemy => respawnTimeDecreasePerEnemy;
        public float RespawnAmountIncreasePerEnemy => respawnAmountIncreasePerEnemy;
        
        public override void InstallBindings()
        {
            Container.Bind<IEnemySpawnSettings>().FromInstance(this).AsSingle();
        }
    }
}