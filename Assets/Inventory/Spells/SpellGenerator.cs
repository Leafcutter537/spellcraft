using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Assets.Combat.SpellEffects;
using Assets.Inventory.Runes;
using Assets.Inventory.Scrolls;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(SpellGenerator), menuName = "ScriptableObjects/SpellGenerator")]
public class SpellGenerator : ScriptableObject
{
    [SerializeField] private RuneGenerator runeGenerator;

    public List<Spell> CreateSpells(List<SpellData> spellData)
    {
        List<Spell> returnList = new List<Spell>();
        foreach (SpellData spellDataIndividual in spellData)
        {
            returnList.Add(CreateSpell(spellDataIndividual));
        }
        return returnList;
    }
    public Spell CreateSpell(SpellData spellData)
    {
        List<Rune> runes = runeGenerator.CreateRunes(spellData.runeData);
        RuneSlotModification[] modifications = GetEnhancementModfiications(runes, spellData.scrollData);
        List<SpellEffect> spellEffects = GetSpellEffects(runes, spellData.scrollData, modifications);
        float manaCost = GetManaCost(runes, spellData.scrollData, modifications);
        /*
        for (int i = 0; i < spellData.runeData.Count; i++)
        {
            if (spellData.runeData[i] != null)
            {
                Rune rune = runeGenerator.CreateRune(spellData.runeData[i]);
                switch (rune.runeData.runeType)
                {
                    case RuneType.Projectile:
                        spellEffects.Add(new CreateProjectile(0, rune.strength, Element.Basic));
                        break;
                }
                manaCost += rune.manaCost;
            }
        }
        */
        return new Spell(spellData, spellEffects, manaCost);
    }
    private RuneSlotModification[] GetEnhancementModfiications(List<Rune> runes, ScrollData scrollData)
    {
        RuneSlotModification[] returnArray = new RuneSlotModification[runes.Count];
        for (int i = 0; i < returnArray.Length; i++)
        {
            returnArray[i] = new RuneSlotModification();
        }
        for (int i = 0; i < runes.Count; i++)
        {
            Rune rune = runes[i];
            if (rune == null)
                continue;
            switch (rune.runeData.runeType)
            {
                case RuneType.StrengthenAdjacent:
                    List<int> connections = GetConnections(i, scrollData);
                    foreach (int connection in connections)
                    {
                        returnArray[connection].strengthPercentage += rune.strength;
                    }
                    break;
            }
        }
        return returnArray;
    }
    private List<int> GetConnections(int index, ScrollData scrollData)
    {
        List<int> returnList = new List<int>();
        for (int i = 0; i < scrollData.connections.Length; i++)
        {
            if (scrollData.connections[i] == index)
            {
                if (i % 2 == 0)
                    returnList.Add(scrollData.connections[i + 1]);
                else
                    returnList.Add(scrollData.connections[i - 1]);
            }
        }
        return returnList;
    }
    private float GetManaCost(List<Rune> runes, ScrollData scrollData, RuneSlotModification[] modifications)
    {
        float returnValue = 0f;
        for (int i = 0; i < runes.Count; i++)
        {
            if (runes[i] != null)
            {
                returnValue += runes[i].manaCost * modifications[i].manaCostMultiplier;
            }
        }
        return returnValue;
    }
    private List<SpellEffect> GetSpellEffects(List<Rune> runes, ScrollData scrollData, RuneSlotModification[] modifications)
    {
        List<SpellEffect> returnList = new List<SpellEffect>();
        for (int i = 0; i < runes.Count; i++)
        {
            if (runes[i] != null)
            {
                switch (runes[i].runeData.runeType)
                {
                    case RuneType.Projectile:
                        returnList.Add(new CreateProjectile(0, runes[i].strength * (1 + modifications[i].strengthPercentage/100f), Element.Basic));
                        break;
                }
            }
        }
        return returnList;
    }
}
