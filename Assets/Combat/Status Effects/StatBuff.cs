using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Combat.SpellEffects
{
    public class StatBuff : StatusEffect
    {
        public int buffStrength;
        public CombatStat stat;

        public StatBuff(int buffStrength, int turnsRemaining, CombatStat stat)
        {
            this.buffStrength = buffStrength;
            this.turnsRemaining = turnsRemaining;
            this.stat = stat;
        }

        public override string GetTitle()
        {
            return PlayerStats.GetCombatStatName(stat) + " Buff (+" + buffStrength + ")";
        }
    }
}
