using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Currency;
using UnityEngine;


namespace Assets.Inventory.Runes
{
    [Serializable]
    public class RuneData
    {
        public int rank;
        public int quality;
        public RuneType runeType;
        public RuneCategory category;
        public RequiredPrimary requiredPrimary;
        public CurrencyType currencyType;

        public RuneData(int rank, int quality, CurrencyType currencyType, RuneType runeType, RuneCategory category, RequiredPrimary requiredPrimary)
        {
            this.rank = rank;
            this.quality = quality;
            this.runeType = runeType;
            this.category = category;
            this.requiredPrimary = requiredPrimary;
            this.currencyType = currencyType;
        }

        public RuneData(RuneData runeData)
        {
            rank = runeData.rank;
            quality = runeData.quality;
            runeType = runeData.runeType;
            category = runeData.category;
            requiredPrimary = runeData.requiredPrimary;
            currencyType = runeData.currencyType;
        }

        public string GetRuneName()
        {
            return System.Text.RegularExpressions.Regex.Replace(Enum.GetName(typeof(RuneType), runeType), "[A-Z]", " $0").Trim();
        }
        public string GetRuneCategoryString()
        {
            return System.Text.RegularExpressions.Regex.Replace(Enum.GetName(typeof(RuneCategory), category), "[A-Z]", " $0").Trim();
        }
    }

    public enum RuneType
    {
        Projectile,
        StrengthenAdjacent,
        Shield,
        Counterspell,
        Heal,
        FireStrengthenAdjacent,
        FrostStrengthenAdjacent,
        ProjectileOneHigher,
        ProjectileOneLower,
        ShieldOneHigher,
        ShieldOneLower,
        BuffProjectilePower,
        BuffShieldPower,
        BuffHealPower
    }
    public enum RuneCategory
    {
        Primary,
        Secondary,
        Enhancement
    }
    public enum RequiredPrimary
    {
        None,
        Projectile,
        Shield
    }

}
