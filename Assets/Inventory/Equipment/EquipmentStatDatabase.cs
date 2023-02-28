using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Equipment
{
    [CreateAssetMenu(fileName = nameof(EquipmentStatDatabase), menuName = "ScriptableObjects/Equipment/EquipmentStatDatabase")]
    public class EquipmentStatDatabase : ScriptableObject
    {
        public List<EquipmentStatData> equipmentStatData;

        public static int GetEquipmentIndex(EquipmentSet set, EquipmentSlot slot)
        {
            int numSlots = Enum.GetNames(typeof(EquipmentSlot)).Length;
            return ((int)set * numSlots) + (int)slot;
        }
    }
}
