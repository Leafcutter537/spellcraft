using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInteractable : Interactable
{
    [SerializeField] private ShopInteractableEvent shopInteractableEvent;
    public override void ShowInteractPanel()
    {
        shopInteractableEvent.Raise(this, null);
    }
}
