using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Assets.Equipment;
using UnityEngine;

public class InventoryEquipmentSelectPanel : SelectPanel
{
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private EquipmentSetDropdownSelect dropdown;

    protected override void Start()
    {
        
    }
    protected override void GetInventory()
    {
        itemList = new List<SelectChoice>();
        for (int i = 0; i < 5; i++) itemList.Add(null);
        if (inventoryController.ownedEquipment.Count > 0)
        {
            List<EquipmentPiece> equipment = inventoryController.GetEquipmentFromSet(dropdown.GetSelectedSet());
            foreach (EquipmentPiece equ in equipment)
            {
                itemList[(int)equ.ownedEquipmentData.equipmentSlot] = equ;
            }
        }
    }
}
