using System.Collections.Generic;
using Assets.Equipment;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class EquipmentSetDropdownSelect : MonoBehaviour
{
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private InventoryEquipmentSelectPanel equipmentSelectPanel;
    private List<EquipmentSet> ownedEquipmentSets;

    private void Start()
    {
        ownedEquipmentSets = GetOwnedEquipmentSets();
        List<string> optionNames = new List<string>();
        foreach (EquipmentSet equipmentSet in ownedEquipmentSets)
        {
            optionNames.Add(Enum.GetName(typeof(EquipmentSet), equipmentSet));
        }
        dropdown.AddOptions(optionNames);
        dropdown.value = 0;
        equipmentSelectPanel.RefreshInventory();
    }

    public List<EquipmentSet> GetOwnedEquipmentSets()
    {
        List<EquipmentSet> returnList = new List<EquipmentSet>();
        foreach (EquipmentPiece ownedEquipment in inventoryController.ownedEquipment)
        {
            if (!returnList.Contains(ownedEquipment.ownedEquipmentData.equipmentSet))
                returnList.Add(ownedEquipment.ownedEquipmentData.equipmentSet);
        }
        return returnList;
    }

    public EquipmentSet GetSelectedSet()
    {
        return ownedEquipmentSets[dropdown.value];
    }

    public void OnValueChanged()
    {
        equipmentSelectPanel.RefreshInventory();
    }

}
