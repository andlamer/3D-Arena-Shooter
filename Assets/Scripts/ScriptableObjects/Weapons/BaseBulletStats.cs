using System.Collections.Generic;
using ShooterBase.Core;
using UnityEngine;

namespace ShooterBase.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Bullets/Create BaseBulletStats", fileName = "BulletStats", order = 1)]
    public class BaseBulletStats : ScriptableObject
    {
        [SerializeField] private bool useAutoDestroy;
        [SerializeField] private float autoDestroyTime = 2f;
        [SerializeField] private float flySpeed = 10f;
        [SerializeField] private string[] hitTags = {"Enemy"};
        [SerializeField] private string[] selfDestroyTags = {"Walls"};
        [SerializeField] private DamageToAffectedStat[] damageToAffectedStats = {new(50, CreatureStats.Health)};

        public bool UseAutoDestroy => useAutoDestroy;
        public float AutoDestroyTime => autoDestroyTime;
        public float FlySpeed => flySpeed;
        public IEnumerable<string> HitTags => hitTags;
        public IEnumerable<string> SelfDestroyTags => selfDestroyTags;
        public IEnumerable<DamageToAffectedStat> DamageToAffectedStats => damageToAffectedStats;
    }
}