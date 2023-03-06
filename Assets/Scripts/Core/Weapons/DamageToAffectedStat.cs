using System;
using UnityEngine;

namespace ShooterBase.Core
{
    [Serializable]
    public class DamageToAffectedStat
    {
        [SerializeField] private int damage = 50;
        [SerializeField] private CreatureStats affectedCreatureStat;
        
        public int Damage => damage;
        public CreatureStats AffectedCreatureStat => affectedCreatureStat;

        public DamageToAffectedStat(int damage, CreatureStats stat)
        {
            this.damage = damage;
            affectedCreatureStat = stat;
        }
        
        public DamageToAffectedStat(){}
    }
}