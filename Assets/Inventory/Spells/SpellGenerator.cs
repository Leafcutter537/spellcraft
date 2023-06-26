using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Assets.Combat.SpellEffects;
using Assets.Inventory.Runes;
using Assets.Inventory.Scrolls;
using Assets.Combat;
using UnityEngine;
using System.Globalization;

namespace Assets.Inventory.Spells
{
    [CreateAssetMenu(fileName = nameof(SpellGenerator), menuName = "ScriptableObjects/Spells/SpellGenerator")]
    public class SpellGenerator : ScriptableObject
    {
        [SerializeField] private RuneGenerator runeGenerator;
        [SerializeField] private SpellIconDatabase spellIconDatabase;
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
            RuneSlotModification[] modifications = GetEnhancementModifications(runes, spellData.scrollData);
            RuneSlotAugmentation[] augmentations = GetAugmentations(runes, spellData.scrollData, modifications);
            List<SpellEffect> spellEffects = GetSpellEffects(runes, spellData.scrollData, modifications, augmentations);
            int cooldown = GetCooldown(runes);
            int chargeTime = GetChargeTime(runes);
            int manaCost = GetManaCost(runes, spellData.scrollData, modifications);
            TargetType targetType = GetTargetType(spellData);
            PlayerSpell returnSpell = new PlayerSpell(spellData, spellEffects, manaCost, chargeTime, cooldown, targetType);
            returnSpell.title = spellData.spellName;
            returnSpell.icon = GetSpellIcon(spellData);
            return returnSpell;
        }
        private RuneSlotAugmentation[] GetAugmentations(List<Rune> runes, ScrollData scrollData, RuneSlotModification[] modifications)
        {
            RuneSlotAugmentation[] returnArray = new RuneSlotAugmentation[runes.Count];
            for (int i = 0; i < returnArray.Length; i++)
            {
                returnArray[i] = new RuneSlotAugmentation();
            }
            for (int i = 0; i < runes.Count; i++)
            {
                Rune rune = runes[i];
                if (rune == null)
                    continue;
                RuneType[] augmentationTypeArray = { RuneType.DebuffHealPower, RuneType.DebuffProjectilePower, RuneType.DebuffShieldPower };
                if (augmentationTypeArray.Contains<RuneType>(rune.runeData.runeType))
                {
                    List<int> connections = GetConnections(i, scrollData);
                    foreach (int connection in connections)
                    {
                        AddAugmentation(returnArray[connection], (1 + modifications[i].strengthPercentage / 100f), rune);
                    }
                }
            }
            return returnArray;
        }
        private void AddAugmentation(RuneSlotAugmentation augmentation, float modificationStrength, Rune rune)
        {
            int augmentationStrength = (int)Mathf.Round(rune.strength * modificationStrength); 
            RuneType[] projectileTypeArray = { RuneType.DebuffHealPower, RuneType.DebuffProjectilePower, RuneType.DebuffShieldPower };
            if (projectileTypeArray.Contains<RuneType>(rune.runeData.runeType))
            {
                ProjectileAugmentation projectileAugmentation = FindExistingProjectileAugmentation(augmentation, rune.runeData.runeType);
                if (projectileAugmentation != null)
                {
                    projectileAugmentation.strength += augmentationStrength;
                }
                else
                    switch (rune.runeData.runeType)
                    {
                        case (RuneType.DebuffHealPower):
                            augmentation.projectileAugmentations.Add(new ApplyDebuff(augmentationStrength, 2, CombatStat.HealPower));
                            break;
                        case (RuneType.DebuffProjectilePower):
                            augmentation.projectileAugmentations.Add(new ApplyDebuff(augmentationStrength, 2, CombatStat.ProjectilePower));
                            break;
                        case (RuneType.DebuffShieldPower):
                            augmentation.projectileAugmentations.Add(new ApplyDebuff(augmentationStrength, 2, CombatStat.ShieldPower));
                            break;
                    }
            }
        }
        private ProjectileAugmentation FindExistingProjectileAugmentation(RuneSlotAugmentation augmentation, RuneType runeType)
        {
            foreach (ProjectileAugmentation projectileAugmentation in augmentation.projectileAugmentations)
            {
                if (ProjectileAugmentationMatchesRuneType(projectileAugmentation, runeType))
                    return projectileAugmentation;
            }
            return null;
        }
        private bool ProjectileAugmentationMatchesRuneType(ProjectileAugmentation augmentation, RuneType runeType)
        {
            if (augmentation is ApplyDebuff applyDebuff)
            {
                if (runeType is RuneType.DebuffHealPower & applyDebuff.stat == CombatStat.HealPower)
                    return true;
                if (runeType is RuneType.DebuffProjectilePower & applyDebuff.stat == CombatStat.ProjectilePower)
                    return true;
                if (runeType is RuneType.DebuffShieldPower & applyDebuff.stat == CombatStat.ShieldPower)
                    return true;
            }
            return false;
        }

        private RuneSlotModification[] GetEnhancementModifications(List<Rune> runes, ScrollData scrollData)
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
                    case RuneType.ManaCostReduction:
                        connections = GetConnections(i, scrollData);
                        foreach (int connection in connections)
                        {
                            returnArray[connection].manaCostMultiplier *= Rune.GetManaCostMultiplier(rune.strength);
                        }
                        break;
                    case RuneType.CooldownIncrease:
                        for (int j = 0; j < runes.Count; j++)
                        {
                            returnArray[j].strengthPercentage += rune.strength;
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
        private int GetCooldown(List<Rune> runes)
        {
            int cooldown = 0;
            foreach (Rune rune in runes)
            {
                if (rune != null)
                {
                    if (rune.runeData.runeType == RuneType.CooldownIncrease)
                        cooldown++;
                    else if (rune.runeData.runeType == RuneType.Heal)
                        cooldown++;
                }
            }
            return cooldown;
        }
        private int GetChargeTime(List<Rune> runes)
        {
            int chargeTime = 0;
            foreach (Rune rune in runes)
            {
                if (rune != null)
                {
                    if (rune.runeData.runeType == RuneType.ChargeUpIncrease)
                        chargeTime++;
                }
            }
            return chargeTime;
        }
        private List<SpellEffect> GetSpellEffects(List<Rune> runes, ScrollData scrollData, RuneSlotModification[] modifications, RuneSlotAugmentation[] augmentations)
        {
            List<SpellEffect> returnList = new List<SpellEffect>();
            for (int i = 0; i < runes.Count; i++)
            {
                if (runes[i] != null)
                {
                    int spellStrength = (int)Mathf.Round(runes[i].strength * (1 + modifications[i].strengthPercentage / 100f));
                    switch (runes[i].runeData.runeType)
                    {
                        case RuneType.Projectile:
                            returnList.Add(new CreateProjectile(0, spellStrength, modifications[i].elementChange, augmentations[i].projectileAugmentations));
                            break;
                        case RuneType.AdjacentProjectile:
                            returnList.Add(new CreateProjectile(1, spellStrength, modifications[i].elementChange, augmentations[i].projectileAugmentations));
                            break;
                        case RuneType.ProjectileOneLower:
                            returnList.Add(new CreateProjectile(-1, spellStrength, modifications[i].elementChange, augmentations[i].projectileAugmentations));
                            break;
                        case RuneType.Shield:
                            returnList.Add(new CreateShield(0, spellStrength, modifications[i].elementChange, 1));
                            break;
                        case RuneType.AdjacentShield:
                            returnList.Add(new CreateShield(1, spellStrength, modifications[i].elementChange, 1));
                            break;
                        case RuneType.ShieldOneLower:
                            returnList.Add(new CreateShield(-1, spellStrength, modifications[i].elementChange, 1));
                            break;
                        case RuneType.Heal:
                            returnList.Add(new Heal(spellStrength));
                            break;
                        case RuneType.BuffProjectilePower:
                            returnList.Add(new ApplyBuff(spellStrength, 2, CombatStat.ProjectilePower));
                            break;
                        case RuneType.BuffShieldPower:
                            returnList.Add(new ApplyBuff(spellStrength, 2, CombatStat.ShieldPower));
                            break;
                        case RuneType.BuffHealPower:
                            returnList.Add(new ApplyBuff(spellStrength, 2, CombatStat.HealPower));
                            break;
                    }
                }
            }
            return returnList;
        }

        private Sprite GetSpellIcon(SpellData spellData)
        {
            return spellIconDatabase.GetSpellIcon(spellData.iconIndex);
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
            return TargetType.Self;
        }

        public bool IsRuneEntryValid(List<RuneData> runeDataList, ScrollData scrollData)
        {
            int primaryCount = 0;
            RuneType primaryType = RuneType.StrengthenAdjacent;
            foreach (RuneData runeData in runeDataList)
            {
                if (runeData != null)
                {
                    if (runeData.category == RuneCategory.Primary)
                    {
                        primaryType = runeData.runeType;
                        primaryCount++;
                    }
                }
            }
            if (primaryCount > 1)
            {
                tooltipWarningEvent.Raise(this, new TooltipWarningEventParameters("Only one primary rune can be in a spell!"));
                return false;
            }
            foreach (RuneData runeData in runeDataList)
            {
                if (runeData != null)
                {
                    switch (runeData.requiredPrimary)
                    {
                        case RequiredPrimary.Projectile:
                            if (primaryType != RuneType.Projectile)
                            {
                                tooltipWarningEvent.Raise(this, new TooltipWarningEventParameters("A Projectile Rune is required for the " + runeData.GetRuneName() + " Rune!"));
                                return false;
                            }
                            break;
                        case RequiredPrimary.Shield:
                            if (primaryType != RuneType.Shield)
                            {
                                tooltipWarningEvent.Raise(this, new TooltipWarningEventParameters("A Shield Rune is required for the " + runeData.GetRuneName() + " Rune!"));
                                return false;
                            }
                            break;
                    }
                }
            }
            List<Rune> runes = runeGenerator.CreateRunes(runeDataList);
            RuneSlotModification[] modifications = GetEnhancementModifications(runes, scrollData);
            if (modifications == null)
            {
                tooltipWarningEvent.Raise(this, new TooltipWarningEventParameters("A slot cannot be modified by more than one element rune!"));
                return false;
            }
            return true;
        }
    }
}
