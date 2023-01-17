using System.Collections;
using System.Collections.Generic;
using Assets.Combat.SpellEffects;
using UnityEngine;

public class Spell : SelectChoice
{
    public SpellData spellData;
    public List<SpellEffect> spellEffects;
    public float manaCost;

    public Spell(SpellData spellData, List<SpellEffect> spellEffects, float manaCost)
    {
        this.spellData = spellData;
        this.spellEffects = spellEffects;
        this.manaCost = manaCost;
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
}
