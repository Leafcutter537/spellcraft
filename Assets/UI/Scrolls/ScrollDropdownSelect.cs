using System.Collections;
using System.Collections.Generic;
using Assets.Currency;
using Assets.Inventory.Scrolls;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ScrollDropdownSelect : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private ScrollDisplay scrollDisplay;
    [SerializeField] private Button createSpellButton;
    [SerializeField] private TextMeshProUGUI createSpellButtonText;
    [Header("Serialized Object References")]
    [SerializeField] private ScrollStock scrollStock;
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private CurrencyDatabase currencyDatabase;

    private void Start()
    {
        List<string> scrollNames = new List<string>();
        foreach (ScrollData scrollData in scrollStock.scrollStock)
        {
            scrollNames.Add(scrollData.scrollName + " (" + currencyDatabase.GetCurrencyString(scrollData.cost) + ")");
        }
        dropdown.AddOptions(scrollNames);
        int currentIndex = 0;
        if (scrollStock.currentIndex < scrollStock.scrollStock.Count)
            currentIndex = scrollStock.currentIndex;
        dropdown.value = currentIndex;
        DisplayScrollChoice();
    }

    public void OnValueChanged()
    {
        DisplayScrollChoice();
    }

    private void DisplayScrollChoice()
    {
        ScrollData scroll = scrollStock.scrollStock[dropdown.value];
        createSpellButton.interactable = inventoryController.CanAfford(scroll.cost);
        createSpellButtonText.text = "Create Spell (" + currencyDatabase.GetCurrencyString(scroll.cost) + ")";
        scrollDisplay.ChooseScroll(scroll);
    }
}
