using UnityEngine;
using Zenject;
using ScriptableObject = Zenject.ScriptableObject;

namespace ShooterBase.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/PlayerSettings", order = 1)]
    public class CharacterSettings : ScriptableObject, ICharacterSettings
    {
        [Header("Movement")]
        [SerializeField] private float xMovementSpeed;
        [SerializeField] private float yMovementSpeed;
        [SerializeField] private float gravityMultiplier;

        [Header("Health")] 
        [SerializeField] private int maxHealthPoints;
        [SerializeField] private int startingHealthPoints;

        [Header("Strength")] 
        [SerializeField] private int maxStrengthPoints;
        [SerializeField] private int startingStrengthPoints;
        [SerializeField] private int ultimateAbilityStrengthCost;
        

        public Vector2 GetPlayerMovementSpeed() => new(xMovementSpeed, yMovementSpeed);
        public float GetPlayerGravityMultiplier() => gravityMultiplier;
        public (int, int) GetMaxAndStartingHealthAmount() => (maxHealthPoints, startingHealthPoints);
        public (int, int) GetMaxAndStartingStrengthAmount() => (maxStrengthPoints, startingStrengthPoints);
        public int GetUltimateAbilityStrengthCost() => ultimateAbilityStrengthCost;

        public override void InstallBindings()
        {
            Container.Bind<ICharacterSettings>().FromInstance(this).AsSingle();
        }
    }
}