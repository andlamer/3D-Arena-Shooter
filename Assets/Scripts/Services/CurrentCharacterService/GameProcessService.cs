using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShooterBase.Services
{
    public class GameProcessService : IGameProcessService
    {
        private readonly ICharacterStatsService _characterStatsService;
        private readonly IEnemiesService _enemiesService;
        
        public async UniTask RestartGame()
        {
            var currentSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
            await SceneManager.LoadSceneAsync(currentSceneBuildIndex, LoadSceneMode.Single);
        }

        public void PauseGame() => Time.timeScale = 0;
        public void ResumeGame() => Time.timeScale = 1;
    }
}