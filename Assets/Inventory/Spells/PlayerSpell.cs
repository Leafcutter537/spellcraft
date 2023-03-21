using System.Collections;
using System.Collections.Generic;
using Assets.Combat.SpellEffects;
using UnityEngine;

namespace Assets.Inventory.Spells
{
    public class PlayerSpell : Spell
    {
        public SpellData spellData;

        public PlayerSpell(SpellData spellData, List<SpellEffect> spellEffects, int manaCost, int chargeTime, int cooldown, TargetType targetType) : base(spellEffects, manaCost, chargeTime, cooldown, targetType)
        {
            this.spellData = spellData;
        }
    }
}
