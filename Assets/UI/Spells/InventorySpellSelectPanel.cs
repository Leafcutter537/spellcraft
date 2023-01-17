using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySpellSelectPanel : SelectPanel
{
    [SerializeField] private InventoryController inventoryController;
    protected override void GetInventory()
    {
        itemList = inventoryController.GetSpellList();
    }
}
