using ShooterBase.Core;
using UnityEngine;
using Zenject;

namespace ShooterBase
{
    public class UIInstaller : MonoInstaller
    {
        [SerializeField] private PauseMenu pauseMenu;
        [SerializeField] private RestartMenu restartMenu;
        [SerializeField] private SimpleTutorialMenu simpleTutorialMenu;

        public override void InstallBindings()
        {
            Container.Bind<PauseMenu>().FromInstance(pauseMenu).AsSingle();
            Container.Bind<RestartMenu>().FromInstance(restartMenu).AsSingle();
            Container.Bind<SimpleTutorialMenu>().FromInstance(simpleTutorialMenu).AsSingle();
        }
    }
}