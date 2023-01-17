using System.Collections;
using System.Collections.Generic;
using Assets.Combat.SpellEffects;
using UnityEngine;

public class RuneSlotModification
{
    public float strengthPercentage;
    public float manaCostMultiplier;
    public Element elementChange;

    public RuneSlotModification()
    {
        manaCostMultiplier = 1;
    }
}
