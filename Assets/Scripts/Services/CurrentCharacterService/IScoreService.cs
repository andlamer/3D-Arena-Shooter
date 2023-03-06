namespace ShooterBase.Services
{
    public interface IScoreService
    {
        void ResetKilledEnemiesCounter();
        void IncreaseKilledEnemiesCounter();
        int GetKilledEnemiesCounter();
    }
}