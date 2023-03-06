using ShooterBase.Services;
using UnityEngine;
using Zenject;

namespace ShooterBase.Core
{
    public class UltimateAbility : MonoBehaviour
    {
        private IEnemiesService _enemiesService;

        [Inject]
        private void Construct(IEnemiesService enemiesService)
        {
            _enemiesService = enemiesService;
        }

        public void Use()
        {
            _enemiesService.WipeAll();
        }
    }
}