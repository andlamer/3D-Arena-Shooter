using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ShooterBase.Core
{
    public class SimpleTutorialMenu : MonoBehaviour
    {
        [SerializeField] private float showTime;

        private const string PrefsKeyName = "TutorialWasShowed";

        private CancellationTokenSource _cancellationTokenSource;

        public async UniTask Show()
        {
            if (PlayerPrefs.HasKey(PrefsKeyName))
                return;

            _cancellationTokenSource = new CancellationTokenSource();
            gameObject.SetActive(true);
            await UniTask.Delay(TimeSpan.FromSeconds(showTime), cancellationToken: _cancellationTokenSource.Token);
            gameObject.SetActive(false);
            PlayerPrefs.SetInt(PrefsKeyName, 1);
        }

        private void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }
    }
}