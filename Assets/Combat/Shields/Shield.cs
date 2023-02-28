using System.Collections;
using System.Collections.Generic;
using Assets.Combat.SpellEffects;
using UnityEngine;

namespace Assets.Combat
{
    public class Shield : MonoBehaviour
    {
        [SerializeField] private CombatLogMessageEvent combatLogMessageEvent;
        // Shield Stats
        [HideInInspector] public int turnsRemaining;
        [HideInInspector] public int strength;
        private Element _element;
        [HideInInspector]
        public Element element
        {
            get { return _element; }
            set
            {
                _element = value;
                UpdateVisuals();
            }
        }
        [Header("Element Visuals")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Color fireColor;
        [SerializeField] private Color frostColor;
        // Names
        [HideInInspector] public string pathName;
        [HideInInspector] public string ownerName;
        public int PredictDamageNegated(int projectileStrength, Element projectileElement)
        {
            NegatedDamage negatedDamage = GetNegatedDamage(this.strength, projectileStrength, this.element, projectileElement);
            return negatedDamage.projectileStrengthLoss;
        }
        public int NegateDamage(Projectile projectile)
        {
            NegatedDamage negatedDamage = GetNegatedDamage(this.strength, projectile.strength, this.element, projectile.element);
            this.strength -= negatedDamage.shieldStrengthLoss;
            string messagedEnd = this.strength > 0 ? "!" : " and was destroyed!";
            string combatMessage = ownerName + "'s shield absorbed " + negatedDamage.projectileStrengthLoss + " damage" + messagedEnd;
            combatLogMessageEvent.Raise(this, new CombatLogEventParameters(combatMessage));
            return negatedDamage.projectileStrengthLoss;
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

        public void UpdateVisuals()
        {
            
            switch (element)
            {
                case (Element.Fire):
                    spriteRenderer.color = fireColor;
                    break;
                case (Element.Frost):
                    spriteRenderer.color = frostColor;
                    break;
            }
        }

        public static NegatedDamage GetNegatedDamage(int shieldStrength, int projectileStrength, Element shieldElement, Element projectileElement)
        {
            NegatedDamage negatedDamage = new NegatedDamage();
            float shieldDamageMultiplier = SpellEffect.GetElementDamageMultiplier(shieldElement, projectileElement);
            if (shieldStrength < projectileStrength * shieldDamageMultiplier)
            {
                negatedDamage.shieldStrengthLoss = shieldStrength;
                negatedDamage.projectileStrengthLoss = (int)(shieldStrength / shieldDamageMultiplier);
            }
            else
            {
                negatedDamage.projectileStrengthLoss = projectileStrength;
                negatedDamage.shieldStrengthLoss = (int)(projectileStrength * shieldDamageMultiplier);
            }
            return negatedDamage;
        }

    }
}
