using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeRuneInfoDisplay : InfoDisplay
{
    [SerializeField] private Button upgradeButton;
    public override void ClearInfo()
    {
        upgradeButton.interactable = false;
    }

    public override void DisplayInfo(SelectChoice selectChoice)
    {
        upgradeButton.interactable = selectChoice != null;
    }
}
