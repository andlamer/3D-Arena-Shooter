using UnityEngine;

namespace ShooterBase.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Bullets/Create PlayerBulletStats", fileName = "PlayerBulletStats", order = 1)]
    public class CharacterBulletStats : BaseBulletStats
    {
        [SerializeField] private float baseEffectChance = 15f;
        [SerializeField] private float maxEffectChance = 100f;
        [SerializeField] private float hpPercentageThresholdForMaxChance = 20f;
        [SerializeField] private float ricochetRadius = 2f;
        [SerializeField] private int ricochetHpRestoreThreshold = 50;
        [SerializeField] private int strengthRestore = 15;

        public float BaseEffectChance => baseEffectChance;
        public float MaxEffectChance => maxEffectChance;
        public float HpPercentageThresholdForMaxChance => hpPercentageThresholdForMaxChance;
        public float RicochetRadius => ricochetRadius;
        public int RicochetHpRestoreThreshold => ricochetHpRestoreThreshold;
        public int StrengthRestore => strengthRestore;
    }
}