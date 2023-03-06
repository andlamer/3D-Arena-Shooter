using ShooterBase.Services;
using UnityEngine;
using Zenject;

namespace ShooterBase.Core
{
    public class EntryPoint : MonoBehaviour
    {
        private Character _character;
        private IRespawnPointsService _respawnPointsService;
        private IEnemiesService _enemiesService;
        private SimpleTutorialMenu _simpleTutorialMenu;

        [Inject]
        private void Construct(Character character, IRespawnPointsService respawnPointsService, IEnemiesService enemiesService, SimpleTutorialMenu simpleTutorialMenu)
        {
            _character = character;
            _respawnPointsService = respawnPointsService;
            _enemiesService = enemiesService;
            _simpleTutorialMenu = simpleTutorialMenu;
        }

        private async void Start()
        {
            RespawnCharacterAtRandomPoint();
            await _simpleTutorialMenu.Show();
            _enemiesService.StartSpawning();
        }

        private void RespawnCharacterAtRandomPoint()
        {
            var characterTransform = _character.transform;
            characterTransform.position = _respawnPointsService.GetRandomPlayerRespawnPoint();
            characterTransform.rotation = Quaternion.identity;
        }
    }
}