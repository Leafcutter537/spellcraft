using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapperInteractable : Interactable
{
    [SerializeField] private ScrapperInteractableEvent scrapperInteractableEvent;
    public override void ShowInteractPanel()
    {
        scrapperInteractableEvent.Raise(this, null);
    }
}
