using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace Assets.Inventory.Runes
{
    public class Rune : SelectChoice
    {
        public Sprite symbolSprite;
        public RuneData runeData;
        public float strength;
        public float manaCost;

        public Rune(Sprite symbolSprite, Sprite icon, RuneData runeData, float strength, float manaCost, string description)
        {
            this.symbolSprite = symbolSprite;
            this.icon = icon;
            this.strength = strength;
            this.manaCost = manaCost;
            this.description = description;
            this.runeData = new RuneData(runeData);
        }

        public override string GetTitle()
        {
            return "Rank " + runeData.rank + " " + runeData.GetRuneName() + " Rune";
        }

        public override string GetDescription()
        {
            return description.Replace("[STRENGTH]", strength.ToString());
        }
    }
}
