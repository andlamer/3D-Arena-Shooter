using ShooterBase.Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ShooterBase.Core
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private Button restartButton;

        private IGameProcessService _gameProcessService;

        [Inject]
        private void Construct(IGameProcessService gameProcessService)
        {
            _gameProcessService = gameProcessService;
        }

        private void Start()
        {
            continueButton.onClick.AddListener(OnContinueButtonClick);
            restartButton.onClick.AddListener(OnRestartButtonClick);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _gameProcessService.PauseGame();
        }

        private void OnRestartButtonClick()
        {
            Hide();
            _gameProcessService.RestartGame();
        }
        
        private void OnContinueButtonClick() => Hide();

        private void Hide()
        {
            _gameProcessService.ResumeGame();
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            continueButton.onClick.RemoveListener(OnContinueButtonClick);
        }
    }
}

