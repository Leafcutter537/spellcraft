using System.Collections;
using System.Collections.Generic;
using Assets.Inventory.Runes;
using Assets.Inventory.Spells;
using TMPro;
using UnityEngine;

public class ConfirmSpellSalvage : ConfirmSelectChoice
{
    private PlayerSpell playerSpell;
    [SerializeField] private RuneGenerator runeGenerator;
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private List<Rune> runes;
    [SerializeField] private TextMeshProUGUI confirmText;
    [SerializeField] private InventorySpellSelectPanel selectPanel;
    public override void TakeSelectChoice(SelectChoice choice)
    {
        playerSpell = choice as PlayerSpell;
        runes = new List<Rune>();
        foreach (RuneData runeData in playerSpell.spellData.runeData)
        {
            runes.Add(runeGenerator.CreateRune(runeData));
        }
        confirmText.text = "Do you wish to salvage this spell? The spell will be removed from your spells and you will receive:\n";
        foreach (Rune rune in runes)
        {
            confirmText.text += rune.GetTitle() + " of quality " + rune.runeData.quality + "\n";
        }
    }

    public void OnConfirm()
    {
        foreach (Rune rune in runes)
        {
            inventoryController.AddRune(rune);
        }
        inventoryController.UnequipSpell(playerSpell);
        inventoryController.RemoveSpell(playerSpell);
        selectPanel.RefreshInventory();
    }
}
