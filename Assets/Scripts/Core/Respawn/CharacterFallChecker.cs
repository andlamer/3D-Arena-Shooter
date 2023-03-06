using System.Linq;
using ShooterBase.Services;
using UnityEngine;
using Zenject;

namespace ShooterBase.Core
{
    [RequireComponent(typeof(Collider))]
    public class CharacterFallChecker : MonoBehaviour
    {
        [SerializeField] private string[] objectTags = {"Player"};

        private IRespawnPointsService _respawnPointsService;
        
        [Inject]
        private void Construct(IRespawnPointsService respawnPointsService)
        {
            _respawnPointsService = respawnPointsService;
        }
        
        
        private void OnTriggerEnter(Collider other)
        {
            if (!objectTags.Contains(other.tag)) return;

            var otherTransform = other.transform;
            otherTransform.position = _respawnPointsService.GetRandomPlayerRespawnPoint();
            otherTransform.rotation = Quaternion.identity;
        }
    }
}