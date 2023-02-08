using System.Collections;
using System.Collections.Generic;
using Assets.Combat.SpellEffects;
using UnityEngine;

namespace Assets.Combat
{
    public class Shield : MonoBehaviour
    {
        [SerializeField] private CombatLogMessageEvent combatLogMessageEvent;
        public int turnsRemaining;
        public int strength;
        public Element element;
        public string pathName;
        public string ownerName;
        public int PredictDamageNegated(int strength, Element element)
        {
            return Mathf.Min(this.strength, strength);
        }
        public int NegateDamage(Projectile projectile)
        {
            int damageNegated = Mathf.Min(projectile.strength, this.strength);
            this.strength -= damageNegated;
            string messagedEnd = this.strength > 0 ? "!" : " and was destroyed!";
            string combatMessage = ownerName + "'s shield absorbed " + damageNegated + " damage" + messagedEnd;
            combatLogMessageEvent.Raise(this, new CombatLogEventParameters(combatMessage));
            return damageNegated;
        }

        public bool ShieldPreventsAllDamage(Projectile projectile)
        {
            return strength >= projectile.strength;
        }

        public bool Advance()
        {
            turnsRemaining--;
            if (turnsRemaining <= 0)
            {
                string combatMessage = ownerName + "'s shield on the " + pathName + "expired!";
                combatLogMessageEvent.Raise(this, new CombatLogEventParameters(combatMessage));
                Destroy(gameObject);
                return true;
            }
            return false;
        }

        public string GetShieldDescription()
        {
            return ownerName + "'s shield of strength " + strength + ". Expires in " + turnsRemaining + " turns.";
        }
    }
}
