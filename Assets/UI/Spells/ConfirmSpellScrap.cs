using System.Collections;
using System.Collections.Generic;
using Assets.Currency;
using Assets.Inventory.Runes;
using Assets.Inventory.Spells;
using TMPro;
using UnityEngine;

public class ConfirmSpellScrap : ConfirmSelectChoice
{
    private PlayerSpell playerSpell;
    [SerializeField] private RuneGenerator runeGenerator;
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private CurrencyDatabase currencyDatabase;
    [SerializeField] private List<Rune> runes;
    [SerializeField] private TextMeshProUGUI confirmText;
    [SerializeField] private InventorySpellSelectPanel selectPanel;
    [SerializeField] private List<CurrencyQuantity> totalValue;
    public override void TakeSelectChoice(SelectChoice choice)
    {
        playerSpell = choice as PlayerSpell;
        runes = new List<Rune>();
        foreach (RuneData runeData in playerSpell.spellData.runeData)
        {
            runes.Add(runeGenerator.CreateRune(runeData));
        }
        List<CurrencyQuantity> valueList = new List<CurrencyQuantity>();
        foreach (Rune rune in runes)
        {
            valueList.Add(new CurrencyQuantity(StoreConstants.GetSellValue(rune), rune.runeData.currencyType));
        }
        totalValue = CurrencyQuantity.CondenseValues(valueList);
        confirmText.text = "Do you wish to scrap this spell? The spell will be removed from your spells and you will receive:\n";
        foreach (CurrencyQuantity currencyQuantity in totalValue)
        {
            CurrencyInfo currencyInfo = currencyDatabase.GetCurrencyInfo(currencyQuantity.currencyType);
            confirmText.text += currencyQuantity.quantity + " " + currencyInfo.currencyName + "\n";
        }
    }

    public void OnConfirm()
    {
        foreach (CurrencyQuantity currencyQuantity in totalValue)
        {
            inventoryController.AddCurrencyQuantity(currencyQuantity);
        }
        inventoryController.UnequipSpell(playerSpell);
        inventoryController.RemoveSpell(playerSpell);
        selectPanel.RefreshInventory();
    }
}
