using System.Collections;
using System.Collections.Generic;
using Assets.Inventory.Runes;
using UnityEngine;

public class SellRuneButton : MonoBehaviour
{
    [SerializeField] private SelectPanelChoice runeChoice;
    [SerializeField] private ConfirmRuneSale confirmRuneSale;

    public void GoToConfirm()
    {
        Rune rune = runeChoice.selectChoice as Rune;
        confirmRuneSale.gameObject.SetActive(true);
        confirmRuneSale.DisplayConfirm(rune);
    }
}
