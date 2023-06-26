using System.Collections;
using System.Collections.Generic;
using Assets.Currency;
using Assets.Equipment;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnchantmentController : MonoBehaviour
{
    [Header("UI Objects")]
    [SerializeField] private InventoryEquipmentSelectPanel inventoryEquipmentSelectPanel;
    [SerializeField] private TextMeshProUGUI currencyAmountText;
    [SerializeField] private TextMeshProUGUI instructionText;
    [SerializeField] private EquipmentSetDropdownSelect equipmentSetDropdownSelect;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private List<EquipmentUpgradeStatLine> statLines;
    [Header("ScriptableObjects")]
    [SerializeField] private EquipmentUpgradeDatabase equipmentUpgradeDatabase;
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private CurrencyDatabase currencyDatabase;
    [SerializeField] private EquipmentStatDatabase equipmentStatDatabase;
    // Equipment Upgrading
    private EquipmentPiece currentPiece;
    private CurrencyQuantity upgradeCost;
    public void UpdateEnchantInfo(EquipmentPiece equipment)
    {
        currentPiece = equipment;
        if (equipment == null)
        {
            instructionText.text = "You have not yet found this equipment piece!";
            upgradeButton.gameObject.SetActive(false);
            SetStatLinesInactive();
        }
        else if (equipment.ownedEquipmentData.currentLevel >= equipment.maxLevel)
        {
            instructionText.text = "This equipment piece is at max level!";
            upgradeButton.gameObject.SetActive(false);
            SetStatLinesInactive();
        }
        else
        {
            SetStatLinesInactive();
            upgradeButton.gameObject.SetActive(true);
            EquipmentUpgradeInfo upgradeInfo = equipmentUpgradeDatabase.GetUpgradeInfo(equipment.ownedEquipmentData.equipmentSet);
            ShowStatInfo(equipment, upgradeInfo);
            upgradeCost = new CurrencyQuantity(upgradeInfo.GetUpgradeCost(equipment.ownedEquipmentData.currentLevel), upgradeInfo.currencyType);
            instructionText.text = "You need " + currencyDatabase.GetCurrencyString(upgradeCost) + " to upgrade this piece.";
            upgradeButton.interactable = inventoryController.CanAfford(upgradeCost);
        }
    }

    public void UpdateCurrencyType()
    {
        EquipmentSet equipmentSet = equipmentSetDropdownSelect.GetSelectedSet();
        CurrencyType currencyType = equipmentUpgradeDatabase.GetUpgradeInfo(equipmentSet).currencyType;
        CurrencyQuantity currencyQuantity = inventoryController.GetCurrencyQuantity(currencyType);
        currencyAmountText.text = "You have " + currencyDatabase.GetCurrencyString(currencyQuantity);
    }

    private void SetStatLinesInactive()
    {
        foreach (EquipmentUpgradeStatLine statLine in statLines)
        {
            statLine.gameObject.SetActive(false);
        }
    }

    private void ShowStatInfo(EquipmentPiece equipment, EquipmentUpgradeInfo upgradeInfo)
    {
        int equipmentIndex = EquipmentStatDatabase.GetEquipmentIndex(equipment.ownedEquipmentData.equipmentSet, equipment.ownedEquipmentData.equipmentSlot);
        EquipmentStatData equipmentStatData = equipmentStatDatabase.equipmentStatData[equipmentIndex];
        int level = equipment.ownedEquipmentData.currentLevel;
        int statLineIndex = 0;
        statLines[statLineIndex].gameObject.SetActive(true);
        statLines[statLineIndex].SetStatLine(level, level + 1, "Level");
        statLineIndex++;
        int[] growths = { equipmentStatData.manaGrowth, equipmentStatData.healthGrowth, equipmentStatData.resilienceGrowth,
        equipmentStatData.projectilePowerGrowth, equipmentStatData.shieldPowerGrowth, equipmentStatData.healPowerGrowth};
        int[] baseValues = { equipmentStatData.baseMana, equipmentStatData.baseHealth, equipmentStatData.baseResilience,
        equipmentStatData.baseResilience, equipmentStatData.baseShieldPower, equipmentStatData.baseHealPower};
        string[] statNames = { "Mana", "Health", "Resilience", "Projectile Power", "Shield Power", "Healing Power" };
        for (int i = 0; i < growths.Length; i++)
        {
            if (growths[i] > 0)
            {
                SetNextStatLine(level, baseValues[i], growths[i], statLineIndex, statNames[i]);
                statLineIndex++;
            }
        }
    }

    private void SetNextStatLine(int currentLevel, int baseValue, int growthValue, int statLineIndex, string statName)
    {
        int currentValue = baseValue + currentLevel * growthValue;
        int newValue = baseValue + (currentLevel + 1) * growthValue;
        statLines[statLineIndex].gameObject.SetActive(true);
        statLines[statLineIndex].SetStatLine(currentValue, newValue, statName);
    }

    public void UpgradeEquipmentPiece()
    {
        inventoryController.SubtractCurrencyQuantity(upgradeCost); 
        int equipmentIndex = EquipmentStatDatabase.GetEquipmentIndex(currentPiece.ownedEquipmentData.equipmentSet, currentPiece.ownedEquipmentData.equipmentSlot);
        EquipmentStatData equipmentStatData = equipmentStatDatabase.equipmentStatData[equipmentIndex];
        currentPiece.ownedEquipmentData.currentLevel++;
        currentPiece.SetStats(currentPiece.ownedEquipmentData, equipmentStatData);
        UpdateEnchantInfo(currentPiece);
    }
}
