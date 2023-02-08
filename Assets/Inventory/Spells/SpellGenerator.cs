using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Assets.Combat.SpellEffects;
using Assets.Inventory.Runes;
using Assets.Inventory.Scrolls;
using UnityEngine;

namespace Assets.Inventory.Spells
{
    [CreateAssetMenu(fileName = nameof(SpellGenerator), menuName = "ScriptableObjects/SpellGenerator")]
    public class SpellGenerator : ScriptableObject
    {
        [SerializeField] private RuneGenerator runeGenerator;
        [SerializeField] private Sprite defaultSpellIcon;
        [SerializeField] private TooltipWarningEvent tooltipWarningEvent;

        public List<PlayerSpell> CreateSpells(List<SpellData> spellData)
        {
            List<PlayerSpell> returnList = new List<PlayerSpell>();
            foreach (SpellData spellDataIndividual in spellData)
            {
                returnList.Add(CreateSpell(spellDataIndividual));
            }
            return returnList;
        }
        public PlayerSpell CreateSpell(SpellData spellData)
        {
            List<Rune> runes = runeGenerator.CreateRunes(spellData.runeData);
            RuneSlotModification[] modifications = GetEnhancementModfiications(runes, spellData.scrollData);
            List<SpellEffect> spellEffects = GetSpellEffects(runes, spellData.scrollData, modifications);
            int manaCost = GetManaCost(runes, spellData.scrollData, modifications);
            TargetType targetType = GetTargetType(spellData);
            PlayerSpell returnSpell = new PlayerSpell(spellData, spellEffects, manaCost, targetType);
            returnSpell.title = spellData.spellName;
            returnSpell.icon = GetSpellIcon(spellData);
            return returnSpell;
        }
        private RuneSlotModification[] GetEnhancementModfiications(List<Rune> runes, ScrollData scrollData)
        {
            List<int> connections = new List<int>();
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
                        connections = GetConnections(i, scrollData);
                        foreach (int connection in connections)
                        {
                            returnArray[connection].strengthPercentage += rune.strength;
                        }
                        break;
                    case RuneType.FireStrengthenAdjacent:
                        connections = GetConnections(i, scrollData);
                        foreach (int connection in connections)
                        {
                            returnArray[connection].strengthPercentage += rune.strength;
                            if (returnArray[connection].elementChange != Element.Basic & returnArray[connection].elementChange != Element.Fire)
                                return null;
                            returnArray[connection].elementChange = Element.Fire;
                        }
                        break;
                    case RuneType.FrostStrengthenAdjacent:
                        connections = GetConnections(i, scrollData);
                        foreach (int connection in connections)
                        {
                            returnArray[connection].strengthPercentage += rune.strength;
                            if (returnArray[connection].elementChange != Element.Basic & returnArray[connection].elementChange != Element.Frost)
                                return null;
                            returnArray[connection].elementChange = Element.Frost;
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
        private int GetManaCost(List<Rune> runes, ScrollData scrollData, RuneSlotModification[] modifications)
        {
            float returnValue = 0f;
            for (int i = 0; i < runes.Count; i++)
            {
                if (runes[i] != null)
                {
                    returnValue += runes[i].manaCost * modifications[i].manaCostMultiplier;
                }
            }
            return (int)Mathf.Round(returnValue);
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
                            int projectileStrength = (int)Mathf.Round(runes[i].strength * (1 + modifications[i].strengthPercentage / 100f));
                            returnList.Add(new CreateProjectile(0, projectileStrength, modifications[i].elementChange));
                            break;
                        case RuneType.Shield:
                            int shieldStrength = (int)Mathf.Round(runes[i].strength * (1 + modifications[i].strengthPercentage / 100f));
                            returnList.Add(new CreateShield(0, shieldStrength, modifications[i].elementChange, 2));
                            break;
                    }
                }
            }
            return returnList;
        }

        private Sprite GetSpellIcon(SpellData spellData)
        {
            return defaultSpellIcon;
        }

        private TargetType GetTargetType(SpellData spellData)
        {
            foreach (RuneData rune in spellData.runeData)
            {
                if (rune != null)
                {
                    switch (rune.runeType)
                    {
                        case RuneType.Projectile:
                            return TargetType.Projectile;
                        case RuneType.Counterspell:
                            return TargetType.Counterspell;
                        case RuneType.Shield:
                            return TargetType.Shield;
                    }
                }
            }
            return TargetType.NoPrimary;
        }

        public bool IsRuneEntryValid(List<RuneData> runeDataList, ScrollData scrollData)
        {
            int primaryCount = 0;
            foreach (RuneData runeData in runeDataList)
            {
                if (runeData != null)
                {
                    if (runeData.category == RuneCategory.Primary)
                    {
                        primaryCount++;
                    }
                }
            }
            if (primaryCount > 1)
            {
                tooltipWarningEvent.Raise(this, new TooltipWarningEventParameters("Only one primary rune can be in a spell!"));
                return false;
            }
            List<Rune> runes = runeGenerator.CreateRunes(runeDataList);
            RuneSlotModification[] modifications = GetEnhancementModfiications(runes, scrollData);
            if (modifications == null)
            {
                tooltipWarningEvent.Raise(this, new TooltipWarningEventParameters("A slot cannot be modified by more than one element rune!"));
                return false;
            }
            return true;
        }
    }
}
