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
            return "Restores " + strength + " health to the caster.";
        }
    }
}
