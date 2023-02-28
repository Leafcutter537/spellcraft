using System.Collections;
using System.Collections.Generic;
using Assets.Currency;
using Assets.EventSystem;
using UnityEngine;

public class CurrencyDisplayController : MonoBehaviour
{
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private CurrencyInfoDisplay currencyInfoDisplay;
    [SerializeField] private CurrencyChangedEvent currencyChangedEvent;

    void Start()
    {
        CurrencyQuantity currencyQuantity = inventoryController.GetCurrencyQuantity(CurrencyType.WorldOneRuneCurrency);
        currencyInfoDisplay.DisplayInfo(currencyQuantity);
    }
    private void OnEnable()
    {
        currencyChangedEvent.AddListener(OnCurrencyChanged);
    }
    private void OnDisable()
    {
        currencyChangedEvent.RemoveListener(OnCurrencyChanged);
    }
    private void OnCurrencyChanged(object sender, EventParameters args)
    {
        CurrencyQuantity currencyQuantity = inventoryController.GetCurrencyQuantity(CurrencyType.WorldOneRuneCurrency);
        currencyInfoDisplay.DisplayInfo(currencyQuantity);
    }
}
