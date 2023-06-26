using System;
using System.Collections.Generic;
using Assets.Currency;
using Assets.Equipment;
using UnityEngine;

public class CurrencyLoader : MonoBehaviour
{
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private DevCurrency devCurrency;
    private static bool hasLoadedInventory;
    private void Awake()
    {
        if (inventoryController.loadDevCurrencyInventory == true & !hasLoadedInventory & Application.isEditor)
        {
            inventoryController.currencyQuantities = new int[Enum.GetValues(typeof(CurrencyType)).Length];
            foreach (CurrencyQuantity currencyQuantity in devCurrency.currencyQuantities)
            {
                inventoryController.AddCurrencyQuantity(currencyQuantity);
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
                inventoryController.currencyQuantities = new int[Enum.GetValues(typeof(CurrencyType)).Length];
                hasLoadedInventory = true;
            }
        }
    }

}
