using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Combat.SpellEffects
{
    public class Heal : SpellEffect
    {
        public int strength;
        public Heal(int strength)
        {
            this.strength = strength;
        }

        public override string GetDescription()
        {
            return GetDescription(null);
        }
        public override string GetDescription(StatBundle bundle)
        {
            string bonusString = "";
            if (bundle != null)
            {
                if (bundle.healPower >= 0)
                    bonusString = " (+" + bundle.healPower + ")";
                else
                    bonusString = " (" + bundle.healPower + ")";
            }
            return "Restores " + strength +  bonusString + " health to the caster.";
        }
    }
}
