using System;
using ShooterBase.Core;

namespace ShooterBase.Services
{
    public interface ICharacterStatsService
    {
        event Action<int> CharacterHpPointsUpdated;
        event Action<int> CharacterStrengthPointsUpdated;

        float GetStatPercent(CreatureStats statType);
        void IncreaseStat(CreatureStats statType, int amount);
        void DecreaseStat(CreatureStats statType, int amount);
        void SetToPercent(CreatureStats statType, int percent);
    }
}