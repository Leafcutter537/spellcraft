using UnityEngine;

namespace Assets.Equipment
{
    public class EquipmentPiece : SelectChoice
    {
        public OwnedEquipmentData ownedEquipmentData;
        public int health { get; private set; }
        public int mana { get; private set; }
        public int resilience { get; private set; }
        public int projectilePower { get; private set; }
        public int shieldPower { get; private set; }
        public int healPower { get; private set; }
        public int maxLevel { get; private set; }
        public EquipmentPiece(OwnedEquipmentData ownedEquipmentData, EquipmentStatDatabase equipmentStatDatabase)
        {
            this.ownedEquipmentData = new OwnedEquipmentData(ownedEquipmentData);
            int index = EquipmentStatDatabase.GetEquipmentIndex(this.ownedEquipmentData.equipmentSet, this.ownedEquipmentData.equipmentSlot);
            EquipmentStatData equipmentStatData = equipmentStatDatabase.equipmentStatData[index];
            this.title = equipmentStatData.equipmentName;
            this.icon = equipmentStatData.sprite;
            this.maxLevel = equipmentStatData.maxLevel;
            SetStats(ownedEquipmentData, equipmentStatData);
        }

        public override string GetDescription()
        {
            string returnString = "";
            if (health != 0) returnString += "Health: " + health + "\n";
            if (mana != 0) returnString += "Mana: " + mana + "\n";
            if (resilience != 0) returnString += "Resilience: " + resilience + "\n";
            if (projectilePower != 0) returnString += "Projectile Power: " + projectilePower + "\n";
            if (shieldPower != 0) returnString += "Shield Power: " + shieldPower + "\n";
            if (healPower != 0) returnString += "Healing Power: " + healPower + "\n";
            return returnString;
        }

        public void SetStats(OwnedEquipmentData ownedEquipmentData, EquipmentStatData equipmentStatData)
        {
            this.health = equipmentStatData.baseHealth + equipmentStatData.healthGrowth * this.ownedEquipmentData.currentLevel;
            this.mana = equipmentStatData.baseMana + equipmentStatData.manaGrowth * this.ownedEquipmentData.currentLevel;
            this.resilience = equipmentStatData.baseResilience + equipmentStatData.resilienceGrowth * this.ownedEquipmentData.currentLevel;
            this.projectilePower = equipmentStatData.baseProjectilePower + equipmentStatData.projectilePowerGrowth * this.ownedEquipmentData.currentLevel;
            this.shieldPower = equipmentStatData.baseShieldPower + equipmentStatData.shieldPowerGrowth * this.ownedEquipmentData.currentLevel;
            this.healPower = equipmentStatData.baseHealPower + equipmentStatData.healPowerGrowth * this.ownedEquipmentData.currentLevel;
        }
    }
}
