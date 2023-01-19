using System.Collections;
using System.Collections.Generic;
using Assets.EventSystem;
using UnityEngine;

public class EquippedSpellSelectPanel : SelectPanel
{
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private EquipSpellEvent equipSpellEvent;
    private void OnEnable()
    {
        equipSpellEvent.AddListener(OnEquipSpell);
    }
    private void OnDisable()
    {
        equipSpellEvent.RemoveListener(OnEquipSpell);
    }
    protected override void GetInventory()
    {
        itemList = inventoryController.GetEquippedSpellList();
    }

    private void OnEquipSpell(object sender, EventParameters args)
    {
        RefreshInventory();
    }
}
