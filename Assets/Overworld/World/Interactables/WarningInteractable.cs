using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningInteractable : Interactable
{
    [SerializeField] private string warningMessage;
    [SerializeField] private WarningInteractableActiveEvent warningInteractableActiveEvent;
    public override void ShowInteractPanel()
    {
        warningInteractableActiveEvent.Raise(this, new WarningInteractableEventParameters(warningMessage));
    }
}
