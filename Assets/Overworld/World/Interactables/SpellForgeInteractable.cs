using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellForgeInteractable : Interactable
{
    [SerializeField] private SpellForgeInteractableEvent spellForgeInteractableEvent;
    public override void ShowInteractPanel()
    {
        spellForgeInteractableEvent.Raise(this, null);
    }
}
