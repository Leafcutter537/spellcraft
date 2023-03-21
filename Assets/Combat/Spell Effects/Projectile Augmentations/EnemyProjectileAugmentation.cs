using System;

namespace Assets.Combat.SpellEffects
{
    [Serializable]
    public class EnemyProjectileAugmentation
    {
        public ProjectileAugmentationType type;
        public int strength;
        public int duration;
        public CombatStat stat;
    }

    public enum ProjectileAugmentationType
    { 
        ApplyDebuff
    }
}
