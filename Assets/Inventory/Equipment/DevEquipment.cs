using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Equipment
{
    [CreateAssetMenu(fileName = nameof(DevEquipment), menuName = "ScriptableObjects/Equipment/DevEquipment")]
    public class DevEquipment : ScriptableObject
    {
        public List<OwnedEquipmentData> ownedEquipmentData;

        public List<OwnedEquipmentData> equippedEquipment;
    }
}
