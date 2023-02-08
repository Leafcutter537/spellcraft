using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Combat.SpellEffects;
using Assets.Inventory.Spells;
using UnityEngine;

namespace Assets.Combat.Enemy
{
    [Serializable]
    public class EnemySpellData
    {
        public string spellName;
        public List<EnemySpellEffect> spellEffects;
        public bool targetsPath;
        public int manaCost;
        public Sprite icon;
        public TargetType targetType;
    }
}
