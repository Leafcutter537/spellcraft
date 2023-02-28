using System.Collections;
using System.Collections.Generic;
using Assets.Inventory.Runes;
using UnityEngine;

public class BuyRuneButton : MonoBehaviour
{
    [SerializeField] private SelectPanelChoice runeChoice;
    [SerializeField] private ConfirmRunePurchase confirmRunePurchase;

    public void GoToConfirm()
    {
        Rune rune = runeChoice.selectChoice as Rune;
        confirmRunePurchase.gameObject.SetActive(true);
        confirmRunePurchase.DisplayConfirm(rune);
    }
}
