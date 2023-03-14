using System.Collections.Generic;
using Assets.Equipment;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Combat
{
    [CreateAssetMenu(fileName = nameof(PlayerStats), menuName = "ScriptableObjects/PlayerStats")]
    public class PlayerStats : ScriptableObject
    {
        public string playerName;
        [Header("Scriptable Object References")]
        [SerializeField] private InventoryController inventoryController;
        [Header("Base Stats")]
        [SerializeField] private int baseHP;
        [SerializeField] private int baseMP;
        [SerializeField] private int baseResilience;
        [SerializeField] private int baseProjectilePower;
        [SerializeField] private int baseShieldPower;
        [SerializeField] private int baseHealPower;

        public int GetCombatStat(CombatStat combatStat)
        {
            List<EquipmentPiece> equipmentList = inventoryController.GetEquippedEquipment();
            int baseValue = 0;
            switch (combatStat)
            {
                case CombatStat.HP:
                    baseValue = baseHP;
                    break;
                case CombatStat.MP:
                    baseValue = baseMP;
                    break;
                case CombatStat.Resilience:
                    baseValue = baseResilience;
                    break;
                case CombatStat.ProjectilePower:
                    baseValue = baseProjectilePower;
                    break;
                case CombatStat.ShieldPower:
                    baseValue = baseShieldPower;
                    break;
                case CombatStat.HealPower:
                    baseValue = baseHealPower;
                    break;
            }
            return baseValue + GetStatBoostFromEquipment(equipmentList, combatStat);
        }

        private int GetStatBoostFromEquipment(List<EquipmentPiece> equipment, CombatStat combatStat)
        {
            int returnSum = 0;
            foreach (EquipmentPiece equipmentPiece in equipment)
            {
                switch (combatStat)
                {
                    case CombatStat.HP:
                        returnSum += equipmentPiece.health;
                        break;
                    case CombatStat.MP:
                        returnSum += equipmentPiece.mana;
                        break;
                    case CombatStat.Resilience:
                        returnSum += equipmentPiece.resilience;
                        break;
                    case CombatStat.ProjectilePower:
                        returnSum += equipmentPiece.projectilePower;
                        break;
                    case CombatStat.ShieldPower:
                        returnSum += equipmentPiece.shieldPower;
                        break;
                    case CombatStat.HealPower:
                        returnSum += equipmentPiece.healPower;
                        break;
                }
            }
            return returnSum;
        }

        public StatBundle GetStatBundle()
        {
            int HP = GetCombatStat(CombatStat.HP);
            int MP = GetCombatStat(CombatStat.MP);
            int resilience = GetCombatStat(CombatStat.Resilience);
            int projectilePower = GetCombatStat(CombatStat.ProjectilePower);
            int shieldPower = GetCombatStat(CombatStat.ShieldPower);
            int healPower = GetCombatStat(CombatStat.HealPower);
            return new StatBundle(HP, MP, resilience, projectilePower, shieldPower, healPower);
        }

        public static string GetCombatStatName(CombatStat stat)
        {
            switch (stat)
            {
                case CombatStat.HP:
                    return "Health";
                case CombatStat.MP:
                    return "Mana";
                case CombatStat.Resilience:
                    return "Resilience";
                case CombatStat.ProjectilePower:
                    return "Projectile Power";
                case CombatStat.ShieldPower:
                    return "Shield Power";
                case CombatStat.HealPower:
                    return "Heal Power";
            }
            return "INVALID STAT";
        }
    }

    public enum CombatStat
    {
        HP,
        MP,
        Resilience,
        ProjectilePower,
        ShieldPower,
        HealPower
    }
}
