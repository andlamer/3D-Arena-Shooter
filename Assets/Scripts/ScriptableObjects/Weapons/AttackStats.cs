using ShooterBase.Core;
using UnityEngine;

namespace ShooterBase.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Attacks/Create AttackStats", fileName = "AttackStats", order = 1)]
    public class AttackStats : ScriptableObject
    {
        [SerializeField] private DamageToAffectedStat[] damageToAffectedStats = {new(15, CreatureStats.Health)};

        public DamageToAffectedStat[] DamageToAffectedStats => damageToAffectedStats;
    }
}