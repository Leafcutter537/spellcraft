using System.Collections;
using System.Collections.Generic;
using Assets.EventSystem;
using Assets.Inventory.Spells;
using UnityEngine;

public class UnequippedSpellSelectPanelChoice : DragPanelChoice
{
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private EquipSpellEvent equipSpellEvent;
    protected override void TransferItem(object sender, EventParameters args)
    {
        if (sender is EquippedSpellSelectPanelChoice sendingPanel)
        {
            if (sendingPanel.selectChoice != null)
            {
                inventoryController.UnequipSpell(sendingPanel.selectChoice as PlayerSpell);
                equipSpellEvent.Raise(this, null);
            }
        }
    }
}
