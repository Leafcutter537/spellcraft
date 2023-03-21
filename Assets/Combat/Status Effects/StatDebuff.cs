using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Combat.SpellEffects
{
    public class StatDebuff : StatusEffect
    {
        public int debuffStrength;
        public CombatStat stat;

        public StatDebuff(int debuffStrength, int turnsRemaining, CombatStat stat)
        {
            this.debuffStrength = debuffStrength;
            this.turnsRemaining = turnsRemaining;
            this.stat = stat;
        }

        public override string GetTitle()
        {
            return PlayerStats.GetCombatStatName(stat) + " Debuff (-" + debuffStrength + ")";
        }
    }
}