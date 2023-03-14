using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Combat.SpellEffects
{
    [Serializable]
    public class EnemySpellEffect
    {
        public int path;
        public int strength;
        public int duration;
        public SpellEffectType spellEffectType;
        public Element element;
        public CombatStat stat;
    }

    public enum SpellEffectType
    {
        CreateProjectile,
        CreateShield,
        Heal,
        Buff
    }
}
