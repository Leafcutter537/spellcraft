using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Combat.SpellEffects
{
    public abstract class SpellEffect 
    {
        public abstract string GetDescription();
    }

    public enum Element
    {
        Basic,
        Fire,
        Frost,
        Light,
        Dark
    }
}
