using UnityEngine;

namespace ShooterBase.ScriptableObjects
{
    public interface ICharacterSettings
    {
        Vector2 GetPlayerMovementSpeed();
        float GetPlayerGravityMultiplier();
        (int, int) GetMaxAndStartingHealthAmount();
        (int, int) GetMaxAndStartingStrengthAmount();
        int GetUltimateAbilityStrengthCost();
    }
}