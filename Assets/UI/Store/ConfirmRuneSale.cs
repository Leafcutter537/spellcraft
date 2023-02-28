using TMPro;
using Assets.Inventory.Runes;
using UnityEngine;
using Assets.Currency;

public class ConfirmRuneSale : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI confirmText;
    [SerializeField] private CurrencyDatabase currencyDatabase;
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private SellRuneSelectPanel sellRuneSelectPanel;
    private Rune rune;
    public void DisplayConfirm(Rune rune)
    {
        this.rune = rune;
        int runeValue = StoreConstants.GetSellValue(rune);
        confirmText.text = "Do you wish to sell " + rune.GetTitle() + " for " + runeValue + " " + rune.currencyName + "?";
    }
    public void OnConfirm()
    {
        int runeValue = StoreConstants.GetSellValue(rune);
        inventoryController.AddCurrencyQuantity(new CurrencyQuantity(runeValue, rune.runeData.currencyType));
        inventoryController.RemoveRune(rune);
        sellRuneSelectPanel.RefreshInventory();
    }
}
