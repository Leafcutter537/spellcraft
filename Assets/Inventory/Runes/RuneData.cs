using System;
using System.Collections;
using System.Collections.Generic;
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
        public RuneSecondaryCategory secondaryCategory;

        public RuneData(int rank, int quality, RuneType runeType, RuneCategory category, RuneSecondaryCategory secondaryCategory)
        {
            this.rank = rank;
            this.quality = quality;
            this.runeType = runeType;
            this.category = category;
            this.secondaryCategory = secondaryCategory;
        }

        public RuneData(RuneData runeData)
        {
            rank = runeData.rank;
            quality = runeData.quality;
            runeType = runeData.runeType;
            category = runeData.category;
            secondaryCategory = runeData.secondaryCategory;
        }

        public string GetRuneName()
        {
            return System.Text.RegularExpressions.Regex.Replace(Enum.GetName(typeof(RuneType), runeType), "[A-Z]", " $0").Trim();
        }
    }

    public enum RuneType
    {
        Projectile,
        StrengthenAdjacent,
        Shield,
        Counterspell,
        FireStrengthenAdjacent,
        FrostStrengthenAdjacent
    }
    public enum RuneCategory
    {
        Primary,
        Secondary,
        Enhancement
    }
    public enum RuneSecondaryCategory
    {
        None,
        Projectile,
        Shield,
        Counterspell,
        Any
    }

}
