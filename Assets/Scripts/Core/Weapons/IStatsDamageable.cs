using System.Collections.Generic;
using ShooterBase.ScriptableObjects;

namespace ShooterBase.Core
{
    internal interface IStatsDamageable
    {
        void TakeDamage(IEnumerable<DamageToAffectedStat> damage);
        bool IsDead();
    }
}