using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace Assets.Inventory.Runes
{
    public class Rune : SelectChoice
    {
        public RuneData runeData;
        public float strength;
        public float manaCost;
        public int value;
        public string currencyName;

        public Rune(Sprite symbolSprite, Sprite icon, RuneData runeData, float strength, float manaCost, int value, string currencyName, string description)
        {
            this.secondaryIcon = symbolSprite;
            this.icon = icon;
            this.strength = strength;
            this.manaCost = manaCost;
            this.value = value;
            this.description = description;
            this.runeData = new RuneData(runeData);
            this.currencyName = currencyName;
        }

        public override string GetTitle()
        {
            return "Rank " + runeData.rank + " " + runeData.GetRuneName() + " Rune";
        }

        public override string GetDescription()
        {
            string returnString = description.Replace("[STRENGTH]", GetStringOfRuneValue(strength));
            returnString = returnString.Replace("[MANACOSTMULTIPLIER]", GetManaCostString(strength));
            return returnString;
        }

        public static string GetStringOfRuneValue(float value)
        {
            return String.Format("{0:0.0}", value);
        }

        public static float GetManaCostMultiplier(float strength)
        {
            return (1 / (1 + strength * 0.01f));
        }

        public static string GetManaCostString(float strength)
        {
            float floatValue = GetManaCostMultiplier(strength);
            return String.Format("{0:0.000}", floatValue);
        }
    }
}
