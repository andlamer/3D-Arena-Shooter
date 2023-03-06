using System;
using System.Runtime.InteropServices;
using ShooterBase.Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ShooterBase.Core
{
    public class CharacterStatsBar : MonoBehaviour
    {
        [SerializeField] private Slider healthBarSlider;
        [SerializeField] private Slider strengthBarSlider;

        private ICharacterStatsService _characterStatsService;

        [Inject]
        private void Construct(ICharacterStatsService characterStatsService)
        {
            _characterStatsService = characterStatsService;
        }
        
        private void Start()
        {
            healthBarSlider.value = _characterStatsService.GetStatPercent(CreatureStats.Health);
            strengthBarSlider.value = _characterStatsService.GetStatPercent(CreatureStats.Strength);

            _characterStatsService.CharacterHpPointsUpdated += OnHealthUpdate;
            _characterStatsService.CharacterStrengthPointsUpdated += OnStrengthUpdate;
        }

        private void OnHealthUpdate(int amount) => healthBarSlider.value = _characterStatsService.GetStatPercent(CreatureStats.Health);
        private void OnStrengthUpdate(int obj) => strengthBarSlider.value = _characterStatsService.GetStatPercent(CreatureStats.Strength);

        private void OnDestroy()
        {
            _characterStatsService.CharacterHpPointsUpdated -= OnHealthUpdate;
            _characterStatsService.CharacterStrengthPointsUpdated -= OnStrengthUpdate;
        }
    }
}