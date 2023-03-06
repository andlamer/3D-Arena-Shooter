using System.Linq;
using ShooterBase.Services;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace ShooterBase.Core
{
    public class RespawnPointsPointsManager : MonoBehaviour, IRespawnPointsManager
    {
        [SerializeField] private RespawnGridCell[] gridCells;

        [Inject]
        private void Construct(IRespawnPointsService respawnPointsService)
        {
            respawnPointsService.SetRespawnManager(this);
        }
        
        
        public Vector3 GetRandomRespawnPointForPlayerWithinGrid()
        {
            var sortedCells = gridCells
                .GroupBy(x => x.CurrentTargetsInCell)
                .OrderBy(x => x.Key)
                .First()
                .ToList();
            var gridCell = sortedCells.ElementAt(Random.Range(0, sortedCells.Count));
            gridCell.AddSpawnedPlayer();

            return gridCell.GetRespawnPosition();
        }
        
        public Vector3 GetRandomSpawnPointForEnemyWithinGrid()
        {
            var sortedCells = gridCells
                .Where(x => !x.ContainsPlayer)
                .GroupBy(x => x.CurrentTargetsInCell)
                .OrderBy(x => x.Key)
                .First()
                .ToList();
            var gridCell = sortedCells.ElementAt(Random.Range(0, sortedCells.Count));
            gridCell.AddSpawnedTarget();
            
            return gridCell.GetRespawnPosition();
        }
    }
}