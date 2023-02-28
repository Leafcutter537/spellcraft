using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Combat.SpellEffects
{
    public abstract class SpellEffect 
    {
        public abstract string GetDescription();
        public abstract string GetDescription(StatBundle bundle);

        public static float GetElementDamageMultiplier(Element shieldElement, Element projectileElement)
        {
            float opposedMultiplier = 2f;
            float sameMultiplier = 0.5f;
            if (shieldElement == projectileElement & shieldElement != Element.Basic) return sameMultiplier;
            if (shieldElement == Element.Fire & projectileElement == Element.Frost) return opposedMultiplier;
            if (shieldElement == Element.Frost & projectileElement == Element.Fire) return opposedMultiplier;
            if (shieldElement == Element.Dark & projectileElement == Element.Light) return opposedMultiplier;
            if (shieldElement == Element.Light & projectileElement == Element.Dark) return opposedMultiplier;
            return 1;
        }
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
