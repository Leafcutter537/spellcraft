using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeRuneSelectPanel : SelectPanel
{
    [SerializeField] private InventoryController inventoryController;
    protected override void GetInventory()
    {
        inventoryController.ClearHeldRunes();
        itemList = inventoryController.GetNonMaxRuneList();
    }
}
