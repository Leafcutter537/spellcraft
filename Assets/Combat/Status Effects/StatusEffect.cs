using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Combat.SpellEffects
{
    public abstract class StatusEffect
    {
        public int turnsRemaining;

        public abstract string GetTitle();
    }
}
