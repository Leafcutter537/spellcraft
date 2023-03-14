
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Assets.Combat.SpellEffects;
using UnityEngine;

namespace Assets.Combat
{
    public class Projectile : MonoBehaviour
    {
        // Projectile Stats
        [HideInInspector] public int strength;
        private Element _element;
        [HideInInspector] public Element element
        {
            get { return _element; }
            set 
            { 
                _element = value;
                UpdateVisuals();
            }
        }
        [HideInInspector] public int turnsToArrive;
        // Scene References
        [HideInInspector] public CharacterInstance target;
        [HideInInspector] public Path path;
        [Header("Projectile Movement")]
        private int movementStep;
        [HideInInspector] public float t;
        private float destinationT;
        [SerializeField] private float deltaT;
        private bool isMoving;
        [SerializeField] private float speed;
        private bool arrivesAtTargetThisTurn;
        [Header("Element Visuals")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Color fireColor;
        [SerializeField] private Color frostColor;
        [Header("Movement Event References")]
        [SerializeField] private StartCombatAnimationEvent startCombatAnimationEvent;
        [SerializeField] private EndCombatAnimationEvent endCombatAnimationEvent;

        private void Update()
        {
            if (isMoving)
            {
                bool isTIncreasing = t > Mathf.PI;
                if ((isTIncreasing & t > destinationT) |
                    (!isTIncreasing & t < destinationT))
                {
                    Arrive();
                }
                else
                {
                    float desiredDistance = speed * Time.deltaTime;
                    float tplus = t;
                    tplus += t > Mathf.PI ? deltaT : -deltaT;
                    Vector2 currentPosition = transform.position;
                    Vector2 newPosition = GetCoordinatesOnEllipse(tplus);
                    float actualDistance = Vector2.Distance(newPosition, currentPosition);
                    t += t > Mathf.PI ? deltaT * desiredDistance / actualDistance : -deltaT * desiredDistance / actualDistance;
                    transform.position = GetCoordinatesOnEllipse(t);
                }
            }
        }
        private Vector2 GetCoordinatesOnEllipse(float t)
        {
            Vector2 coordinates = path.GetCoordinatesOnEllipse(t);
            if (target is PlayerInstance)
                coordinates.x *= -1;
            return coordinates;
        }

        // Advances the projectile one timestep. Returns true if the Projectile reached its destination
        // on this timestep and should be removed from the Path.
        public bool Advance()
        {
            turnsToArrive--;
            MovementAnimation();
            if (turnsToArrive == 0)
            {
                arrivesAtTargetThisTurn = true;
                return true;
            }
            return false;
        }
        public void MovementAnimation()
        {
            startCombatAnimationEvent.Raise(this, null);
            destinationT = path.GetDestinationT(movementStep);
            isMoving = true;
        }

        private void Arrive()
        {
            if (arrivesAtTargetThisTurn)
            {
                strength -= path.GetNegatedDamage(target is EnemyInstance, this);
                if (strength > 0)
                    target.ReceiveProjectile(this);
                Destroy(gameObject);
            }
            endCombatAnimationEvent.Raise(this, null);
            movementStep++;
            isMoving = false;
        }

        public string GetProjectileDescription()
        {
            string elementString = Enum.GetName(typeof(Element), element );
            string direction = target is PlayerInstance ? "Incoming " : "Outgoing ";
            string turnsString = turnsToArrive == 1 ? " turn." : " turns.";
            turnsString = turnsToArrive.ToString() + turnsString;
            return direction + elementString + " projectile of strength " + strength.ToString() + "; arrives in " + turnsString;
        }

        public int PredictEnemyShieldEffectiveness(int shieldStrength, Element shieldElement, int shieldDuration)
        {
            if (shieldDuration < turnsToArrive)
                return 0;
            NegatedDamage negatedDamage = Shield.GetNegatedDamage(shieldStrength, this.strength, shieldElement, this.element);
            return negatedDamage.projectileStrengthLoss;
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
    }
}