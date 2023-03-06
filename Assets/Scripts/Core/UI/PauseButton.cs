using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ShooterBase.Core
{
    public class PauseButton : MonoBehaviour
    {
        [SerializeField] private Button pauseButton;

        private PauseMenu _pauseMenu;

        [Inject]
        private void Construct(PauseMenu pauseMenu)
        {
            _pauseMenu = pauseMenu;
        }
        
        private void Start()
        {
            pauseButton.onClick.AddListener(OnPauseButtonClick);
        }

        private void OnPauseButtonClick() => _pauseMenu.Show();

        private void OnDestroy()
        {
            pauseButton.onClick.RemoveAllListeners();
        }
    }
}