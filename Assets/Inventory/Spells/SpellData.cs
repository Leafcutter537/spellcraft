using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Inventory.Runes;
using Assets.Inventory.Scrolls;
using UnityEngine;

[Serializable]
public class SpellData
{
    public ScrollData scrollData;
    public List<RuneData> runeData;
    
    public SpellData(ScrollData scrollData, List<RuneData> runeData)
    {
        this.scrollData = scrollData; 
        this.runeData = runeData;
    }
}