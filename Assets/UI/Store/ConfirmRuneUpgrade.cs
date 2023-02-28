using System.Collections;
using System.Collections.Generic;
using TMPro;
using Assets.Inventory.Runes;
using UnityEngine;
using Assets.Currency;
using UnityEngine.UI;

public class ConfirmRuneUpgrade : MonoBehaviour
{
    [Header ("UI References")]
    [SerializeField] private UpgradeRuneSelectPanel upgradeRuneSelectPanel;
    [SerializeField] private TextMeshProUGUI originalQualityText;
    [SerializeField] private TextMeshProUGUI originalStrengthText;
    [SerializeField] private TextMeshProUGUI originalManaCostText;
    [SerializeField] private TextMeshProUGUI newQualityText;
    [SerializeField] private TextMeshProUGUI newStrengthText;
    [SerializeField] private TextMeshProUGUI newManaCostText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI runeNameText;
    [SerializeField] private TextMeshProUGUI targetLevelDisplay;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button targetLevelIncreaseButton;
    [SerializeField] private Button targetLevelDecreaseButton;
    [Header("Scriptable Object References")]
    [SerializeField] private RuneGenerator runeGenerator;
    [SerializeField] private InventoryController inventoryController;
    // So what are we upgrading, and how much?
    private int targetLevel;
    private Rune rune;
    private CurrencyQuantity runeCost;
    public void DisplayConfirm(Rune rune)
    {
        this.rune = rune;
        targetLevel = rune.runeData.quality + 1;
        originalQualityText.text = rune.runeData.quality.ToString();
        originalStrengthText.text = Rune.GetStringOfRuneValue(rune.strength);
        originalManaCostText.text = Rune.GetStringOfRuneValue(rune.manaCost);
        runeNameText.text = rune.GetTitle();
        DisplayTargetLevel();
    }
    private void DisplayTargetLevel()
    {
        targetLevelDisplay.text = targetLevel.ToString();
        int value = runeGenerator.GetRuneUpgradeCost(rune.runeData.runeType, rune.runeData.rank, rune.runeData.quality, targetLevel);
        costText.text = "Cost: " + value + " " + rune.currencyName;
        runeCost = new CurrencyQuantity(value, rune.runeData.currencyType);
        newQualityText.text = targetLevel.ToString();
        newStrengthText.text = Rune.GetStringOfRuneValue(runeGenerator.GetNewStrength(rune.runeData, targetLevel));
        newManaCostText.text = Rune.GetStringOfRuneValue(runeGenerator.GetNewManaCost(rune.runeData, targetLevel));
        if (inventoryController.CanAfford(runeCost))
            confirmButton.interactable = true;
        else
            confirmButton.interactable = false;
        targetLevelIncreaseButton.interactable = targetLevel < RuneConstants.MaxRuneQuality;
        targetLevelDecreaseButton.interactable = targetLevel > (rune.runeData.quality + 1);
    }
    public void IncreaseTargetLevel()
    {
        targetLevel++;
        DisplayTargetLevel();
    }
    public void DecreaseTargetLevel()
    {
        targetLevel--;
        DisplayTargetLevel();
    }
    public void OnConfirm()
    {
        rune.runeData.quality = targetLevel;
        runeGenerator.UpdateRuneValues(rune);
        inventoryController.SubtractCurrencyQuantity(runeCost);
        upgradeRuneSelectPanel.RefreshInventory();
    }
}
