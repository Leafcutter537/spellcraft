using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Currency;
using UnityEngine;

namespace Assets.Inventory.Scrolls
{
    [Serializable]
    public class ScrollData
    {
        public CurrencyQuantity cost;
        public string scrollName;
        public int[] numPerLevel;
        public int[] connections;
    }
}