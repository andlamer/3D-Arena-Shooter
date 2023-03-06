using UnityEngine;

namespace ShooterBase.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Weapons/Create WeaponStats", fileName = "WeaponStats", order = 1)]
    public class WeaponStats : ScriptableObject
    {
        [SerializeField] private float weaponCooldown;

        public float WeaponCooldown => weaponCooldown;
    }
}