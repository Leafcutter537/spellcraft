using System;
using Assets.Currency;
using UnityEngine;

namespace Assets.Equipment
{
    [Serializable]
    public class EquipmentUpgradeInfo
    {
        public CurrencyType currencyType;
        [SerializeField] private int baseCost;
        [SerializeField] private int costSlope;

        public int GetUpgradeCost(int currentEquipmentLevel)
        {
            return baseCost + costSlope * currentEquipmentLevel;
        }
    }
}
