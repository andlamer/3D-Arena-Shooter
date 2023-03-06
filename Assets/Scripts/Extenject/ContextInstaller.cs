using ShooterBase.Core;
using ShooterBase.Services;
using UnityEngine;
using Zenject;

namespace ShooterBase
{
    public class ContextInstaller : MonoInstaller
    {
        [SerializeField] private SimpleBullet simpleBulletItem;
        [SerializeField] private HomingBullet homingBulletItem;
        [SerializeField] private DivingEnemy divingEnemyPrefab;
        [SerializeField] private BossEnemy bossEnemyPrefab;
        [SerializeField] private Character characterPrefab;

        public override void InstallBindings()
        {
            Container.BindMemoryPool<SimpleBullet, SimpleBullet.Pool>()
                .WithInitialSize(15)
                .FromComponentInNewPrefab(simpleBulletItem);
            Container.BindMemoryPool<HomingBullet, HomingBullet.Pool>()
                .WithInitialSize(15)
                .FromComponentInNewPrefab(homingBulletItem);
            Container.BindMemoryPool<DivingEnemy, DivingEnemy.Pool>()
                .WithInitialSize(8)
                .FromComponentInNewPrefab(divingEnemyPrefab);
            Container.BindMemoryPool<BossEnemy, BossEnemy.Pool>()
                .WithInitialSize(2)
                .FromComponentInNewPrefab(bossEnemyPrefab);

            Container.BindInterfacesTo<CharacterStatsService>().AsSingle();
            Container.Bind<IRespawnPointsService>().To<RespawnPointsService>().AsSingle();
            Container.BindInterfacesTo<EnemiesService>().AsSingle();
            Container.Bind<IScoreService>().To<ScoreService>().AsSingle();
            Container.Bind<IGameProcessService>().To<GameProcessService>().AsSingle();
            Container.Bind<Character>().FromComponentInNewPrefab(characterPrefab).AsSingle();
        }
    }
}