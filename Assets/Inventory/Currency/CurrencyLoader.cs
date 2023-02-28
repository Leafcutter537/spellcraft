using System;
using System.Collections.Generic;
using Assets.Currency;
using UnityEngine;

public class CurrencyLoader : MonoBehaviour
{
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private DevCurrency devCurrency;
    private static bool hasLoadedDevInventory;
    private void Awake()
    {
        if (inventoryController.loadDevCurrencyInventory == true & !hasLoadedDevInventory & Application.isEditor)
        {
            inventoryController.currencyQuantities = new int[Enum.GetValues(typeof(CurrencyType)).Length];
            foreach (CurrencyQuantity currencyQuantity in devCurrency.currencyQuantities)
            {
                inventoryController.AddCurrencyQuantity(currencyQuantity);
            }
            hasLoadedDevInventory = true;
        }
    }

}
