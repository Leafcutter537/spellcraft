using System.Collections;
using System.Collections.Generic;
using Assets.Combat.SpellEffects;
using UnityEngine;

namespace Assets.Inventory.Spells
{
    public class Spell : SelectChoice
    {
        public List<SpellEffect> spellEffects;
        public int manaCost;
        public TargetType targetType;
        public Spell(List<SpellEffect> spellEffects, int manaCost, TargetType targetType)
        {
            this.spellEffects = spellEffects;
            this.manaCost = manaCost;
            this.targetType = targetType;
        }
        public override string GetDescription()
        {
            string returnString = "";
            foreach (SpellEffect spellEffect in spellEffects)
            {
                returnString += spellEffect.GetDescription() + "\n\n";
            }
            return returnString;
        }
        public string GetDescription(StatBundle statBundle)
        {
            string returnString = "";
            foreach (SpellEffect spellEffect in spellEffects)
            {
                returnString += spellEffect.GetDescription(statBundle) + "\n\n";
            }
            return returnString;
        }
    }

    public enum TargetType
    {
        NoPrimary,
        Projectile,
        Shield,
        Counterspell,
        Heal,
        Buff,
        Self
    }
}
