using System.Collections;
using System.Collections.Generic;
using TMPro;
using Assets.Inventory.Runes;
using UnityEngine;
using Assets.Currency;
using System.Runtime.InteropServices;

public class ConfirmRunePurchase : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI confirmText;
    [SerializeField] private CurrencyDatabase currencyDatabase;
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private BuyRuneSelectPanel buyRuneSelectPanel;
    private Rune rune;
    public void DisplayConfirm(Rune rune)
    {
        this.rune = rune;
        confirmText.text = "Do you wish to buy " + rune.GetTitle() + " for " + rune.value + " " +  rune.currencyName + "?";
    }
    public void OnConfirm()
    {
        inventoryController.SubtractCurrencyQuantity(new CurrencyQuantity(rune.value, rune.runeData.currencyType));
        inventoryController.AddRune(rune);
        buyRuneSelectPanel.RefreshInventory();

    }
}
