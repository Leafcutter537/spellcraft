using System;
using UnityEngine;

namespace Assets.Equipment
{
    [Serializable]
    public class EquipmentStatData
    {
        public string equipmentName;

        public Sprite sprite;

        public int baseMana;
        public int baseHealth;
        public int baseResilience;
        public int baseProjectilePower;
        public int baseShieldPower;
        public int baseHealPower;

        public int manaGrowth;
        public int healthGrowth;
        public int resilienceGrowth;
        public int projectilePowerGrowth;
        public int shieldPowerGrowth;
        public int healPowerGrowth;

        public int maxLevel;
    }

    public enum EquipmentSet
    { 
        Initiate,
        Invoker
    }
    public enum EquipmentSlot
    {
        Head,
        Chest,
        Legs,
        Feet,
        Hands
    }
}