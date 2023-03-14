using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Combat.SpellEffects
{
    public class ApplyBuff : SpellEffect
    {
        public int buffStrength;
        public int duration;
        public CombatStat stat;
        public ApplyBuff(int buffStrength, int duration, CombatStat stat)
        {
            this.buffStrength = buffStrength;
            this.duration = duration;
            this.stat = stat;
        }

        public override string GetDescription()
        {
            return GetDescription(null);
        }
        public override string GetDescription(StatBundle bundle)
        {
            string plural = duration > 1 ? "s" : "";
            return "Increases the caster's " + PlayerStats.GetCombatStatName(stat) + " by " + buffStrength + " for " + duration.ToString() + " turn" + plural + ".";
        }
    }
}