
using System;
using Assets.Currency;
using Assets.Equipment;
using Assets.Inventory.Runes;
using Assets.Inventory.Scrolls;

namespace Assets.Progression
{
    [Serializable]
    public class RewardData
    {
        public RuneData[] runeRewards;
        public OwnedEquipmentData[] equipmentRewards;
        public CurrencyQuantity[] currencyRewards;
        public ScrollData[] scrollUnlocks;
    }
}

