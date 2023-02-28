using System.Collections;
using System.Collections.Generic;
using Assets.Inventory.Runes;
using UnityEngine;

public static class StoreConstants
{
    public static float SellValue = 0.8f;
    public static float UpgradeValue = 1.6f;

    public static int GetSellValue(Rune rune)
    {
        return (int)(rune.value * SellValue);
    }

    public static int GetUpgradeCost(int baseCost)
    {
        return (int)(baseCost * UpgradeValue);
    }
}
