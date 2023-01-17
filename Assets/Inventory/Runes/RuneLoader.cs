using Assets.Inventory.Runes;
using UnityEngine;

public class RuneLoader : MonoBehaviour
{
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private RuneGenerator runeGenerator;
    [SerializeField] private DevRuneInventory devRuneInventory;
    private static bool hasLoadedDevInventory;
    private void Awake()
    {
        if (inventoryController.loadDevRuneInventory == true & !hasLoadedDevInventory)
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
            hasLoadedDevInventory = true;
        }
    }

}
