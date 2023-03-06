using UnityEngine;
using ScriptableObject = Zenject.ScriptableObject;

namespace ShooterBase.ScriptableObjects
{
    [CreateAssetMenu(fileName = "EnemyStats", menuName = "ScriptableObjects/EnemyStats", order = 1)]
    public class EnemyStats : ScriptableObject
    {
        [Header("Health")] 
        [SerializeField] private int maxHealthPoints;
        [SerializeField] private int startingHealthPoints;
        [SerializeField] private int minHealthPoints = 0;

        [Header("AI")] 
        [SerializeField] private float targetAIUpdateTime = 0.1f;
        [SerializeField] private float steeringAISpeed = 1f;
        [SerializeField] private float acceleration = 0.3f;

        [Header("Additional")] 
        [SerializeField] private int onKillCharacterStrengthRecovery = 15;


        public int MaxHealthPoints => maxHealthPoints;
        public int StartingHealthPoints => startingHealthPoints;
        public int MinHealthPoints => minHealthPoints;
        public float TargetAIUpdateTime => targetAIUpdateTime;
        public float SteeringAISpeed => steeringAISpeed;
        public float Acceleration => acceleration;
        public int OnKillCharacterStrengthRecovery => onKillCharacterStrengthRecovery;
    }
}