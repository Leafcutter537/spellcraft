using System.Collections;
using System.Collections.Generic;
using Assets.EventSystem;
using UnityEngine;

public class EquippedSpellSelectPanelChoice : DragPanelChoice
{
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private EquipSpellEvent equipSpellEvent;
    protected override void TransferItem(object sender, EventParameters args)
    {
        if (sender is UnequippedSpellSelectPanelChoice sendingPanel)
        {
            if (sendingPanel.selectChoice != null)
            {
                inventoryController.EquipSpell(sendingPanel.selectChoice as Spell);
                equipSpellEvent.Raise(this, null);
            }
        }
    }


}
