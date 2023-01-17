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

        public RuneData(int rank, int quality, RuneType runeType)
        {
            this.rank = rank;
            this.quality = quality;
            this.runeType = runeType;
        }

        public RuneData(RuneData runeData)
        {
            rank = runeData.rank;
            quality = runeData.quality;
            runeType = runeData.runeType;
        }

        public string GetRuneName()
        {
            return System.Text.RegularExpressions.Regex.Replace(Enum.GetName(typeof(RuneType), runeType), "[A-Z]", " $0").Trim();
        }
    }

    public enum RuneType
    {
        Projectile,
        StrengthenAdjacent
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
