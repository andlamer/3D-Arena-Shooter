using ShooterBase.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ShooterBase.Core
{
    public class RestartMenu : MonoBehaviour
    {
        [SerializeField] private TMP_Text killCounter;
        [SerializeField] private Button restartButton;

        private const string KillCounterFormat = "Total Kills: {0}";

        private IGameProcessService _gameProcessService;
        private IScoreService _scoreService;

        [Inject]
        private void Construct(IGameProcessService gameProcessService, IScoreService scoreService)
        {
            _gameProcessService = gameProcessService;
            _scoreService = scoreService;
        }

        private void Awake()
        {
            restartButton.onClick.AddListener(OnRestartButtonClick);
        }

        private void OnDestroy()
        {
            restartButton.onClick.RemoveAllListeners();
        }

        public void Show()
        {
            killCounter.text = string.Format(KillCounterFormat, _scoreService.GetKilledEnemiesCounter());
            gameObject.SetActive(true);
            _gameProcessService.PauseGame();
        }

        private void OnRestartButtonClick()
        {
            _gameProcessService.ResumeGame();
            _gameProcessService.RestartGame();
        }
    }
}