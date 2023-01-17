using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedSpellSelectPanel : SelectPanel
{
    [SerializeField] private InventoryController inventoryController;
    protected override void GetInventory()
    {
        itemList = inventoryController.GetEquippedSpellList();
    }
}
