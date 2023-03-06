using ShooterBase.Core;
using UnityEngine;

namespace ShooterBase.Services
{
    public class RespawnPointsService : IRespawnPointsService
    {
        private RespawnPointsPointsManager _respawnPointsPointsManager;

        public void SetRespawnManager(RespawnPointsPointsManager respawnPointsPointsManager) => _respawnPointsPointsManager = respawnPointsPointsManager;  
        
        public Vector3 GetRandomPlayerRespawnPoint() => _respawnPointsPointsManager.GetRandomRespawnPointForPlayerWithinGrid();
        public Vector3 GetRandomEnemySpawnPoint() => _respawnPointsPointsManager.GetRandomSpawnPointForEnemyWithinGrid();
    }
}