
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
        public List<ProjectileAugmentation> augmentations;
        // Scene References
        [HideInInspector] public CharacterInstance target;
        [HideInInspector] public GridSquare square;
        [HideInInspector] public GridController gridController;
        [Header("Projectile Movement")]
        private bool isMoving;
        [SerializeField] private float speed;
        float targetX;
        [Header("Element Visuals")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [Header("Movement Event References")]
        [SerializeField] private StartCombatAnimationEvent startCombatAnimationEvent;
        [SerializeField] private EndCombatAnimationEvent endCombatAnimationEvent;
        [Header("Arrow Indicator")]
        [SerializeField] private GameObject arrowPrefab;
        [SerializeField] private float arrowOffset;
        [SerializeField] private Color playerArrowColor;
        [SerializeField] private Color enemyArrowColor;
        private GameObject arrowObject;

        private void Update()
        {
            if (isMoving)
            {
                bool isMovingRight = IsMovingRight();
                if ((isMovingRight & transform.position.x > targetX) |
                    (!isMovingRight & transform.position.x < targetX))
                {
                    Arrive();
                }
                else
                {
                    float moveDistance = speed * Time.deltaTime;
                    moveDistance *= isMovingRight ? 1 : -1;
                    transform.position += new Vector3(moveDistance, 0, 0);
                }
            }
        }

        // Advances the projectile one timestep. Returns true if the Projectile reached its destination
        // on this timestep and should be removed from the Path.
        public bool Advance()
        {
            bool isMovingRight = IsMovingRight();
            if (isMovingRight)
                square.playerProjectile = null;
            else
                square.enemyProjectile = null;
            RemoveArrow();
            square = gridController.GetNextSquare(square, IsMovingRight());
            StartMovementAnimation();
            if (square == null)
            {
                targetX = target.transform.position.x;
                return true;
            }
            else
            {
                if (isMovingRight)
                    square.playerProjectile = this;
                else
                    square.enemyProjectile = this;
                targetX = square.transform.position.x;
                return square.shield != null | gridController.IsEdgeSquare(square);
            }
        }
        public void StartMovementAnimation()
        {
            startCombatAnimationEvent.Raise(this, null);
            isMoving = true;
        }

        private void Arrive()
        {
            if (square == null)
            {
                target.ReceiveProjectile(this);
                Destroy(gameObject);
            }
            else
            {
                bool shouldCreateArrow = true;
                if (square.shield != null)
                {
                    this.strength -= square.shield.NegateDamage(this);
                    if (this.strength <= 0)
                    {
                        shouldCreateArrow = false;
                        Destroy(gameObject);
                    }
                }
                if (gridController.GetNextSquare(square, IsMovingRight()) == null & this.strength > 0)
                {
                    shouldCreateArrow = false;
                    Advance();
                    return;
                }
                if (shouldCreateArrow)
                    CreateArrow();
            }
            endCombatAnimationEvent.Raise(this, null);
            isMoving = false;
        }

        public string GetProjectileDescription()
        {
            string elementString = Enum.GetName(typeof(Element), element );
            string direction = target is PlayerInstance ? "Incoming " : "Outgoing ";
            string augmentationString = "";
            foreach (ProjectileAugmentation augmentation in augmentations)
            {
                augmentationString += "\n" + augmentation.GetDescription();
            }
            return direction + elementString + " projectile of strength " + strength.ToString() + "." + augmentationString;
        }

        public int PredictEnemyShieldEffectiveness(int shieldStrength, Element shieldElement, int shieldDuration)
        {
            return 0;
        }

        public void UpdateVisuals()
        {
            
        }

        private bool IsMovingRight()
        {
            return target is EnemyInstance;
        }

        public void CreateArrow()
        {
            bool isMovingRight = IsMovingRight();
            Destroy(arrowObject);
            Vector3 offset = new Vector3(0, arrowOffset);
            offset.y *= isMovingRight ? -1 : 1;
            arrowObject = Instantiate(arrowPrefab, transform.position + offset, Quaternion.identity);
            SpriteRenderer spriteRenderer = arrowObject.GetComponent<SpriteRenderer>();
            if (!isMovingRight)
                spriteRenderer.flipX = true; 
            spriteRenderer.color = isMovingRight ? playerArrowColor : enemyArrowColor;
        }

        public void RemoveArrow()
        {
            Destroy(arrowObject);
        }
    }
}