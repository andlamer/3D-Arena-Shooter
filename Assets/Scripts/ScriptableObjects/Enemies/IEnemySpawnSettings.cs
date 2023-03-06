namespace ShooterBase.ScriptableObjects
{
    public interface IEnemySpawnSettings
    {
        float MinRespawnTime { get; }
        float MaxRespawnTime { get; }
        float DivingToBossRatio { get; }
        float RespawnTimeDecreasePerEnemy { get; }
        float RespawnAmountIncreasePerEnemy { get; }
    }
}