using System;
using System.Collections.Generic;
using ShooterBase.Core;
using ShooterBase.ScriptableObjects;
using Zenject;

namespace ShooterBase.Services
{
    public class CharacterStatsService : ICharacterStatsService, IInitializable, IDisposable
    {
        public event Action<int> CharacterHpPointsUpdated;
        public event Action<int> CharacterStrengthPointsUpdated;

        private readonly ICharacterSettings _characterSettings;
        private readonly RestartMenu _restartMenu;
        
        private Dictionary<CreatureStats, CharacterStat> _characterStatsDictionary;

        public CharacterStatsService(ICharacterSettings characterSettings, RestartMenu restartMenu)
        {
            _characterSettings = characterSettings;
            _restartMenu = restartMenu;
        }

        public void Initialize()
        {
            _characterStatsDictionary = new Dictionary<CreatureStats, CharacterStat>
            {
                {
                    CreatureStats.Health,
                    new CharacterStat(_characterSettings.GetMaxAndStartingHealthAmount().Item1,
                        _characterSettings.GetMaxAndStartingHealthAmount().Item2,
                        0)
                },
                {
                    CreatureStats.Strength,
                    new CharacterStat(_characterSettings.GetMaxAndStartingStrengthAmount().Item1,
                        _characterSettings.GetMaxAndStartingStrengthAmount().Item2,
                        0)
                }
            };

            _characterStatsDictionary[CreatureStats.Health].MinStatReached += OnZeroHealthPoints;
            _characterStatsDictionary[CreatureStats.Strength].MaxStatReached += OnMaxStrength;
            _characterStatsDictionary[CreatureStats.Health].StatUpdated += OnHealthUpdate;
            _characterStatsDictionary[CreatureStats.Strength].StatUpdated += OnStrengthUpdate;
        }

        public void Dispose()
        {
            _characterStatsDictionary[CreatureStats.Health].MinStatReached -= OnZeroHealthPoints;
            _characterStatsDictionary[CreatureStats.Strength].MaxStatReached -= OnMaxStrength;
            _characterStatsDictionary[CreatureStats.Health].StatUpdated -= OnHealthUpdate;
            _characterStatsDictionary[CreatureStats.Strength].StatUpdated -= OnStrengthUpdate;
        }

        public float GetStatPercent(CreatureStats statType)
        {
            if (_characterStatsDictionary.ContainsKey(statType))
                return _characterStatsDictionary[statType].GetStatPercent();

            return 0f;
        }
        
        public void IncreaseStat(CreatureStats statType, int amount)
        {
            if (_characterStatsDictionary.ContainsKey(statType))
                _characterStatsDictionary[statType].IncreaseStat(amount);
        }

        public void DecreaseStat(CreatureStats statType, int amount)
        {
            if (_characterStatsDictionary.ContainsKey(statType))
                _characterStatsDictionary[statType].DecreaseStat(amount);
        }

        public void SetToPercent(CreatureStats statType, int percent)
        {
            if(_characterStatsDictionary.ContainsKey(statType))
                _characterStatsDictionary[statType].SetToPercent(percent);
        }

        private void OnHealthUpdate(int amount) => CharacterHpPointsUpdated?.Invoke(amount);
        private void OnStrengthUpdate(int amount) => CharacterStrengthPointsUpdated?.Invoke(amount);

        private void OnZeroHealthPoints() => _restartMenu.Show();

        private void OnMaxStrength()
        {
            
        }
    }
}
