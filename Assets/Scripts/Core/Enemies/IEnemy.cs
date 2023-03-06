using System;

namespace ShooterBase.Core
{
    public interface IEnemy
    {
        event Action<IEnemy> OnEnemyDead;
        void Kill();
    }
}