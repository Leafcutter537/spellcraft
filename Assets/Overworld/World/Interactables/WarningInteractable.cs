using System.Collections;
using System.Collections.Generic;
using Language.Lua;
using TMPro;
using UnityEngine;

public class WarningInteractable : Interactable
{
    [SerializeField] private string warningMessage;
    [SerializeField] private TextMeshProUGUI warningText;
    public override void ShowInteractPanel()
    {
        base.ShowInteractPanel();
        warningText.text = warningMessage;
    }
}
