using System.Collections;
using System.Collections.Generic;
using Assets.Inventory.Runes;
using UnityEngine;

public class UpgradeRuneButton : MonoBehaviour
{
    [SerializeField] private SelectPanelChoice runeChoice;
    [SerializeField] private ConfirmRuneUpgrade confirmRuneUpgrade;

    public void GoToConfirm()
    {
        Rune rune = runeChoice.selectChoice as Rune;
        confirmRuneUpgrade.gameObject.SetActive(true);
        confirmRuneUpgrade.DisplayConfirm(rune);
    }
}
