using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace ShooterBase.Core
{
    public class RespawnGridCell : MonoBehaviour
    {
        [SerializeField] private Transform respawnPoint;
        [SerializeField] private float respawnRadius = 1f;
        [SerializeField] private string[] targetTags = {"Enemy"};
        [SerializeField] private string[] playerTags = {"Player"};

        private const int MaxColliders = 15;

        public bool ContainsPlayer { get; private set; }
        public int CurrentTargetsInCell { get; private set; }

        private void Start()
        {
            var colliders = new Collider[MaxColliders];
            Physics.OverlapBoxNonAlloc(gameObject.transform.position, transform.localScale / 2, colliders, Quaternion.identity);

            foreach (var objectCollider in colliders.Where(x => x != null))
            {
                if (playerTags.Contains(objectCollider.tag))
                    ContainsPlayer = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (targetTags.Contains(other.tag))
                CurrentTargetsInCell += 1;

            if (playerTags.Contains(other.tag))
                ContainsPlayer = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (targetTags.Contains(other.tag))
                CurrentTargetsInCell -= 1;

            if (playerTags.Contains(other.tag))
                ContainsPlayer = false;
        }

        public Vector3 GetRespawnPosition() => GetRandomNavmeshLocation(respawnPoint.position, respawnRadius);
        public void AddSpawnedTarget() => CurrentTargetsInCell += 1;
        public void AddSpawnedPlayer() => ContainsPlayer = true;

        private static Vector3 GetRandomNavmeshLocation(Vector3 startingPosition, float radius)
        {
            var randomDirection = Random.insideUnitSphere * radius;
            var finalPosition = Vector3.zero;
            randomDirection += startingPosition;

            if (NavMesh.SamplePosition(randomDirection, out var hit, radius, 1))
                finalPosition = hit.position;

            return finalPosition;
        }
    }
}