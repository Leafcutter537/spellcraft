using System.Collections;
using System.Collections.Generic;
using Assets.Currency;
using Assets.Equipment;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(EquipmentUpgradeDatabase), menuName = "ScriptableObjects/Equipment/EquipmentUpgradeDatabase")]
public class EquipmentUpgradeDatabase : ScriptableObject
{
    [SerializeField] private List<EquipmentSet> equipmentSet;
    [SerializeField] private List<EquipmentUpgradeInfo> upgradeInfo;

    public EquipmentUpgradeInfo GetUpgradeInfo(EquipmentSet set)
    {
        int index = equipmentSet.IndexOf(set);
        return upgradeInfo[index];
    }
}
