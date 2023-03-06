using UnityEngine;

namespace ShooterBase.Core
{
    public interface IRespawnPointsManager
    {
        Vector3 GetRandomRespawnPointForPlayerWithinGrid();
    }
}