using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Inventory.Runes;
using Unity.VisualScripting;
using System.Linq;

[CreateAssetMenu(fileName = nameof(InventoryController), menuName = "ScriptableObjects/InventoryController")]
public class InventoryController : ScriptableObject
{
    [Header("Dev Rune Inventory")]
    public bool loadDevRuneInventory;
    public int devRuneInventoryIndex;
    [Header("Dev Spell Inventory")]
    public bool loadDevSpellInventory;
    public int devSpellInventoryIndex;
    [Header("Inventory Items")]
    public List<Rune> runes;
    public List<Spell> spells;
    [Header("Items Set Aside")]
    [SerializeField] private HoldRuneEvent holdRuneEvent;
    private List<Rune> onHoldRunes;
    public List<Spell> equippedSpells;

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
    public List<SelectChoice> GetSpellList()
    {
        List<SelectChoice> returnList = new List<SelectChoice>();
        foreach (Spell spell in spells)
        {
            returnList.Add(spell);
        }
        return returnList;
    }
    public List<SelectChoice> GetUnequippedSpellList()
    {
        List<SelectChoice> returnList = new List<SelectChoice>();
        foreach (Spell spell in spells)
        {
            if (!(equippedSpells.Contains(spell)))
                returnList.Add(spell);
        }
        return returnList;
    }
    public List<SelectChoice> GetEquippedSpellList()
    {
        List<SelectChoice> returnList = new List<SelectChoice>();
        foreach (Spell spell in equippedSpells)
        {
            returnList.Add(spell);
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

    public bool HasSpellWithName(string name)
    {
        foreach (Spell spell in spells)
        {
            if (spell.title == name)
                return true;
        }
        return false;
    }
    public void EquipSpell(Spell spell)
    {
        equippedSpells.Add(spell);
    }
    public void UnequipSpell(Spell spell)
    {
        equippedSpells.Remove(spell);
    }
}