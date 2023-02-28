using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Inventory.Runes;
using Assets.Inventory.Spells;
using Unity.VisualScripting;
using System.Linq;
using Assets.Progression;
using Assets.Equipment;
using Assets.Currency;

[CreateAssetMenu(fileName = nameof(InventoryController), menuName = "ScriptableObjects/Inventory/InventoryController")]
public class InventoryController : ScriptableObject
{
    [Header("Dev Rune Inventory")]
    public bool loadDevRuneInventory;
    public int devRuneInventoryIndex;
    [Header("Dev Spell Inventory")]
    public bool loadDevSpellInventory;
    public int devSpellInventoryIndex;
    [Header("Dev Equipment Inventory")]
    public bool loadDevEquipmentInventory;
    [Header("Dev Currency Inventory")]
    public bool loadDevCurrencyInventory;
    [Header("Scriptable Object References")]
    [SerializeField] private RuneGenerator runeGenerator;
    [SerializeField] private EquipmentStatDatabase equipmentStatDatabase;
    public CurrencyDatabase currencyDatabase;
    [Header("Event References")]
    [SerializeField] private CurrencyChangedEvent currencyChangedEvent;
    [SerializeField] private EquipmentChangeEvent equipmentChangeEvent;
    [SerializeField] private TooltipWarningEvent toolTipWarningEvent;
    [Header("Inventory Items")]
    public List<Rune> runes;
    public List<PlayerSpell> spells;
    public List<EquipmentPiece> ownedEquipment;
    public int[] currencyQuantities;
    [Header("Items Set Aside")]
    [SerializeField] private HoldRuneEvent holdRuneEvent;
    private List<Rune> onHoldRunes;
    public List<PlayerSpell> equippedSpells;
    [SerializeField] private int maxNumberEquippedSpells;
    public List<int> equippedEquipmentIndices;

    #region Runes
    public List<SelectChoice> GetRuneList()
    {
        List<SelectChoice> returnList = new List<SelectChoice>();
        foreach (Rune rune in runes)
        {
            if (!onHoldRunes.Contains(rune))
            {
                returnList.Add(rune);
            }
        }
        return returnList;
    }
    public List<SelectChoice> GetNonMaxRuneList()
    {
        List<SelectChoice> returnList = new List<SelectChoice>();
        foreach (Rune rune in runes)
        {
            if (rune.runeData.quality < RuneConstants.MaxRuneQuality)
            {
                returnList.Add(rune);
            }
        }
        return returnList;
    }
    public void ClearHeldRunes()
    {
        onHoldRunes = new List<Rune>();
        holdRuneEvent.Raise(this, null);
    }
    public void RemoveHeldRunesFromInventory()
    {
        foreach (Rune rune in onHoldRunes)
        {
            runes.Remove(rune);
        }
        onHoldRunes = new List<Rune>();
    }
    public void HoldRune(Rune rune)
    {
        onHoldRunes.Add(rune);
        holdRuneEvent.Raise(this, null);
    }
    public void UnholdRune(Rune rune)
    {
        onHoldRunes.Remove(rune);
        holdRuneEvent.Raise(this, null);
    }
    public void AddRune(Rune rune)
    {
        runes.Add(rune);
    }
    public void RemoveRune(Rune rune)
    {
        runes.Remove(rune);
    }

    #endregion

    #region Spells
    public List<SelectChoice> GetSpellList()
    {
        List<SelectChoice> returnList = new List<SelectChoice>();
        foreach (PlayerSpell spell in spells)
        {
            returnList.Add(spell);
        }
        return returnList;
    }
    public List<SelectChoice> GetUnequippedSpellList()
    {
        List<SelectChoice> returnList = new List<SelectChoice>();
        foreach (PlayerSpell spell in spells)
        {
            if (!(equippedSpells.Contains(spell)))
                returnList.Add(spell);
        }
        return returnList;
    }
    public List<SelectChoice> GetEquippedSpellList()
    {
        List<SelectChoice> returnList = new List<SelectChoice>();
        foreach (PlayerSpell spell in equippedSpells)
        {
            returnList.Add(spell);
        }
        return returnList;
    }
    public bool HasSpellWithName(string name)
    {
        foreach (PlayerSpell spell in spells)
        {
            if (spell.title == name)
                return true;
        }
        return false;
    }
    public void EquipSpell(PlayerSpell spell)
    {
        if (equippedSpells.Count < maxNumberEquippedSpells)
            equippedSpells.Add(spell);
        else
            toolTipWarningEvent.Raise(this, new TooltipWarningEventParameters("You can only have " + maxNumberEquippedSpells + " spells equipped at a time!"));
    }
    public void UnequipSpell(PlayerSpell spell)
    {
        equippedSpells.Remove(spell);
    }

    public void RemoveSpell(PlayerSpell spell)
    {
        spells.Remove(spell);
    }

    #endregion

    public string AddRewards(RewardData rewardData)
    {
        string returnText = "";
        foreach (RuneData runeData in rewardData.runeRewards)
        {
            Rune rune = runeGenerator.CreateRune(runeData);
            returnText += "Received a " + rune.GetTitle() + " rune of quality " + runeData.quality + "!\n\n";
            runes.Add(rune);
        }
        foreach (OwnedEquipmentData equipmentData in rewardData.equipmentRewards)
        {
            if (GetIndexOfEquipment(equipmentData.equipmentSet, equipmentData.equipmentSlot) == -1)
            {
                EquipmentPiece newPiece = new EquipmentPiece(equipmentData, equipmentStatDatabase);
                newPiece.ownedEquipmentData.currentLevel = 1;
                returnText += "Received " + newPiece.GetTitle() + "!\n\n";
                ownedEquipment.Add(newPiece);
            }
        }
        foreach (CurrencyQuantity currencyQuantity in rewardData.currencyRewards)
        {
            AddCurrencyQuantity(currencyQuantity);
            CurrencyInfo currencyInfo = currencyDatabase.GetCurrencyInfo(currencyQuantity.currencyType);
            returnText += "Found " + currencyQuantity.quantity + " " + currencyInfo.currencyName + "!";
        }
        return returnText;
    }

    #region Equipment
    public List<EquipmentPiece> GetEquipmentFromSet(EquipmentSet equipmentSet)
    {
        List<EquipmentPiece> returnList = new List<EquipmentPiece>();
        foreach (EquipmentPiece equipment in ownedEquipment)
        {
            if (equipment.ownedEquipmentData.equipmentSet == equipmentSet)
                returnList.Add(equipment);
        }
        return returnList;
    }
    public int GetIndexOfEquipment(EquipmentSet equipmentSet, EquipmentSlot equipmentSlot)
    {
        for (int i = 0; i < ownedEquipment.Count; i++)
        {
            OwnedEquipmentData equipment = ownedEquipment[i].ownedEquipmentData;
            if (equipment.equipmentSet == equipmentSet & equipment.equipmentSlot == equipmentSlot)
                return i;
        }
        return -1;
    }
    public List<EquipmentPiece> GetEquippedEquipment()
    {
        List<EquipmentPiece> returnList = new List<EquipmentPiece>();
        foreach (int equipmentIndex in equippedEquipmentIndices)
        {
            returnList.Add(ownedEquipment[equipmentIndex]);
        }
        return returnList;
    }

    public void EquipEquipment(EquipmentPiece equipment)
    {
        EquipmentSlot slot = equipment.ownedEquipmentData.equipmentSlot;
        List<EquipmentPiece> equippedEquipment = GetEquippedEquipment();
        foreach (EquipmentPiece equipmentItem in equippedEquipment)
        {
            if (equipmentItem.ownedEquipmentData.equipmentSlot == slot)
                UnequipEquipment(equipmentItem);
        }
        equippedEquipmentIndices.Add(ownedEquipment.IndexOf(equipment));
        equipmentChangeEvent.Raise(this, null);
    }

    public void UnequipEquipment(EquipmentPiece equipment)
    {
        int equipmentIndex = ownedEquipment.IndexOf(equipment);
        equippedEquipmentIndices.Remove(equipmentIndex);
        equipmentChangeEvent.Raise(this, null);
    }

    #endregion

    #region Currency

    public CurrencyQuantity GetCurrencyQuantity(CurrencyType currencyType)
    {
        return new CurrencyQuantity(currencyQuantities[(int)currencyType], currencyType);
    }

    public void AddCurrencyQuantity(CurrencyQuantity currencyQuantity)
    {
        currencyQuantities[(int)currencyQuantity.currencyType] += currencyQuantity.quantity;
        currencyChangedEvent.Raise(this, null);
    }

    public bool SubtractCurrencyQuantity(CurrencyQuantity currencyQuantity)
    {
        if (!CanAfford(currencyQuantity))
            return false;
        currencyQuantities[(int)currencyQuantity.currencyType] -= currencyQuantity.quantity;
        currencyChangedEvent.Raise(this, null);
        return true;
    }

    public bool CanAfford(CurrencyQuantity currencyQuantity)
    {
        return (currencyQuantities[(int)currencyQuantity.currencyType] >= (currencyQuantity.quantity));
    }

    #endregion

}