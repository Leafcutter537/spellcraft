using System.Collections.Generic;
using Assets.Inventory.Runes;
using Unity.VisualScripting;
using UnityEngine;

public class RuneLoader : MonoBehaviour
{
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private RuneGenerator runeGenerator;
    [SerializeField] private DevRuneInventory devRuneInventory;
    private static bool hasLoadedInventory;
    private void Awake()
    {
        if (inventoryController.loadDevRuneInventory == true & !hasLoadedInventory & Application.isEditor)
        {
            switch (inventoryController.devRuneInventoryIndex)
            {
                case 0:
                    inventoryController.runes = runeGenerator.CreateRunes(devRuneInventory.firstRuneList);
                    break;
                case 1:
                    inventoryController.runes = runeGenerator.CreateRunes(devRuneInventory.secondRuneList);
                    break;
            }
            hasLoadedInventory = true;
        }
        else if (!hasLoadedInventory)
        {
            if (SaveManager.HasSaveData())
            {
                // Load Save
            }
            else
            {
                inventoryController.runes = new List<Rune>();
                hasLoadedInventory = true;
            }
        }

    }

}
