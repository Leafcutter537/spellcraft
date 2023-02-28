using System;

namespace Assets.Equipment
{
    [Serializable]
    public class OwnedEquipmentData
    {
        public EquipmentSet equipmentSet;
        public EquipmentSlot equipmentSlot;
        public int currentLevel;
        public float progressToNextLevel;

        public OwnedEquipmentData(OwnedEquipmentData existing)
        {
            equipmentSet = existing.equipmentSet;
            equipmentSlot = existing.equipmentSlot;
            currentLevel = existing.currentLevel;
            progressToNextLevel = existing.progressToNextLevel;
        }
    }
}