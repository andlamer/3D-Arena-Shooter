using ShooterBase.Core;
using UnityEngine;

namespace ShooterBase.Services
{
    public interface IRespawnPointsService
    {
        void SetRespawnManager(RespawnPointsPointsManager respawnPointsPointsManager);
        Vector3 GetRandomPlayerRespawnPoint();
        Vector3 GetRandomEnemySpawnPoint();
    }
}