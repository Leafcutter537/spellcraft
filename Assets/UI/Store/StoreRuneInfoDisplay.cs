using System.Collections;
using System.Collections.Generic;
using Assets.Currency;
using Assets.Inventory.Runes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreRuneInfoDisplay : InfoDisplay
{
    [SerializeField] private Button buyButton;
    [SerializeField] private Button sellButton;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private bool isSellRune;
    public override void ClearInfo()
    {
        if (buyButton != null)         
            buyButton.interactable = false;
        if (sellButton != null)
            sellButton.interactable = false;
        priceText.text = "";
    }

    public override void DisplayInfo(SelectChoice selectChoice)
    {
        Rune rune = selectChoice as Rune;
        CurrencyQuantity runeCost = new CurrencyQuantity(rune.value, rune.runeData.currencyType);
        if (buyButton != null)
        {
            if (inventoryController.CanAfford(runeCost))
                buyButton.interactable = true;
        }
        if (!isSellRune)
        {
            priceText.text = rune.value + " " + rune.currencyName;
        }
        else
        {
            priceText.text = StoreConstants.GetSellValue(rune) + " " + rune.currencyName;
        }
    }
}
