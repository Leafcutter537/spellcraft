using System.Collections;
using System.Collections.Generic;
using Assets.Inventory.Runes;
using UnityEngine;

namespace Assets.Inventory.Spells
{
    public class SpellLoader : MonoBehaviour
    {
        [SerializeField] private InventoryController inventoryController;
        [SerializeField] private SpellGenerator spellGenerator;
        [SerializeField] private DevSpellInventory devSpellInventory;
        private static bool hasLoadedDevInventory;
        private void Awake()
        {
            if (inventoryController.loadDevSpellInventory == true & !hasLoadedDevInventory & Application.isEditor)
            {
                switch (inventoryController.devSpellInventoryIndex)
                {
                    case 0:
                        inventoryController.spells = spellGenerator.CreateSpells(devSpellInventory.firstSpellList);
                        break;
                    case 1:
                        inventoryController.spells = spellGenerator.CreateSpells(devSpellInventory.secondSpellList);
                        break;
                }
                inventoryController.equippedSpells = new List<PlayerSpell>();
                int maxIndex = Mathf.Min(inventoryController.spells.Count, 3);
                for (int i = 0; i < maxIndex; i++)
                {
                    inventoryController.EquipSpell(inventoryController.spells[i]);
                }
                hasLoadedDevInventory = true;
            }
        }
    }
}