using System.Collections;
using System.Collections.Generic;
using Assets.Combat;
using UnityEngine;

public abstract class ProjectileAugmentation 
{
    public int strength;
    public abstract void EnactAugmentationEffect(CharacterInstance target);

    public abstract string GetDescription();
}
