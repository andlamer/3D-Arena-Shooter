using Cysharp.Threading.Tasks;

namespace ShooterBase.Services
{
    public interface IGameProcessService
    {
        UniTask RestartGame();
        void PauseGame();
        void ResumeGame();
    }
}