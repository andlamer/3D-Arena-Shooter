using System;
using ShooterBase.ScriptableObjects;
using ShooterBase.Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ShooterBase.Core
{
    public class UltimateButtonNotifier : MonoBehaviour
    {
        [SerializeField] private Button ultimateButton;

        private ICharacterStatsService _characterStatsService;
        private ICharacterSettings _characterSettings;

        [Inject]
        private void Construct(ICharacterStatsService characterStatsService, ICharacterSettings characterSettings)
        {
            _characterStatsService = characterStatsService;
            _characterSettings = characterSettings;
        }

        private void Start()
        {
            _characterStatsService.CharacterStrengthPointsUpdated += OnStrengthUpdate;
        }

        private void OnStrengthUpdate(int strengthAmount) => ultimateButton.gameObject.SetActive(strengthAmount >= _characterSettings.GetUltimateAbilityStrengthCost());
        
        private void OnDestroy()
        {
            _characterStatsService.CharacterStrengthPointsUpdated -= OnStrengthUpdate;
        }
    }
}