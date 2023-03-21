using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Combat.SpellEffects
{
    [Serializable]
    public class EnemySpellEffect
    {
        public SpellEffectType spellEffectType;
        [DrawIf("spellEffectType", SpellEffectType.Buff, true)]
        [DrawIf("spellEffectType", SpellEffectType.Heal, true)]
        public int path;
        public int strength;
        [DrawIf("spellEffectType", SpellEffectType.CreateProjectile, true)]
        [DrawIf("spellEffectType", SpellEffectType.Heal, true)]
        public int duration;
        [DrawIf("spellEffectType", SpellEffectType.Buff, true)]
        [DrawIf("spellEffectType", SpellEffectType.Heal, true)]
        public Element element;
        [DrawIf("spellEffectType", SpellEffectType.Buff)]
        public CombatStat stat;
        public List<EnemyProjectileAugmentation> projectileAugmentations;
    }

    public enum SpellEffectType
    {
        CreateProjectile,
        CreateShield,
        Heal,
        Buff
    }
}
