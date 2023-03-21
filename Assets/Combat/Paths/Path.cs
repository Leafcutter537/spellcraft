using System.Collections;
using System.Collections.Generic;
using Assets.Combat.SpellEffects;
using Assets.EventSystem;
using Assets.Inventory.Spells;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Combat
{
    public class Path : MonoBehaviour
    {
        [Header("Name")]
        public string pathName;
        [Header("Visuals")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [Header("Event References")]
        [SerializeField] private MouseEnterPathEvent mouseEnterPathEvent;
        [SerializeField] private MouseExitPathEvent mouseExitPathEvent;
        [SerializeField] private EnterTooltipEvent enterTooltipEvent;
        [SerializeField] private ExitTooltipEvent exitTooltipEvent;
        [SerializeField] private PathSelectEvent pathSelectEvent;
        [SerializeField] private TooltipWarningEvent tooltipWarningEvent;
        [Header("Path Controller")]
        [SerializeField] private PathController pathController;
        [Header("Ellipse")]
        [SerializeField] private float ellipseHeight;
        [SerializeField] private float ellipseWidth;
        [SerializeField] private List<float> projectilePath;
        [SerializeField] private float shieldPoint; 
        [Header("Projectile")]
        public int turnsToArrive;
        [HideInInspector] public Projectile playerProjectile;
        [HideInInspector] public Projectile enemyProjectile;
        [Header("Shields")]
        [HideInInspector] public Shield playerShield;
        [HideInInspector] public Shield enemyShield;
        // Ghost Effects
        private List<GameObject> ghostEffects;
        // Input System
        private PlayerInputActions playerInputActions;
        private InputAction click;
        private bool isHovering;

        private void Awake()
        {
            playerInputActions = new PlayerInputActions();
            ghostEffects = new List<GameObject>();
        }
        private void OnEnable()
        {
            click = playerInputActions.UI.Click;
            click.Enable();
            click.performed += OnClick;
        }
        private void OnDisable()
        {
            click.Disable();
        }
        private void OnDrawGizmosSelected()
        {
            float t = 0;
            float deltaT = 0.05f;
            while (t < Mathf.PI * 2)
            {
                Vector2 firstVector = GetCoordinatesOnEllipse(t);
                t += deltaT;
                Vector2 secondVector = GetCoordinatesOnEllipse(t);
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(firstVector, secondVector);
            }
            Gizmos.color = Color.red;
            foreach (float projectilePosition in projectilePath)
            {
                Gizmos.DrawSphere(GetCoordinatesOnEllipse(projectilePosition), 0.1f);
            }
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(GetCoordinatesOnEllipse(shieldPoint), 0.15f);
        }
        public Vector2 GetCoordinatesOnEllipse(float t)
        {
            float a = ellipseWidth / 2;
            float b = ellipseHeight / 2;
            return new Vector2(a * Mathf.Cos(t), b * Mathf.Sin(t));
        }

        private void OnMouseEnter()
        {
            if (!pathController.enemyDetailsPanel.gameObject.activeInHierarchy & !pathController.turnController.combatIsEnded & !pathController.tutorialController.isShowingTutorial)
            {
                isHovering = true;
                spriteRenderer.color = pathController.hoverColor;
                mouseEnterPathEvent.Raise(this, null);
                enterTooltipEvent.Raise(this, null);
            }
        }

        private void OnMouseExit()
        {
            isHovering = false;
            spriteRenderer.color = pathController.defaultColor;
            mouseExitPathEvent.Raise(this, null);
            exitTooltipEvent.Raise(this, null);
        }
        private void OnClick(InputAction.CallbackContext context)
        {
            if (isHovering)
            {
                Spell spell = pathController.combatSpellSelectPanel.GetSelected() as Spell;
                if (spell != null)
                {
                    if (spell.targetType == TargetType.Projectile & playerProjectile != null)
                    {
                        tooltipWarningEvent.Raise(this, new TooltipWarningEventParameters("You can only have one projectile traveling on a path at a time!"));
                    }
                    else if (spell.targetType == TargetType.Shield & playerShield != null)
                    {
                        tooltipWarningEvent.Raise(this, new TooltipWarningEventParameters("You can only have one shield on a path at a time!"));
                    }
                    else
                    {
                        pathSelectEvent.Raise(this, null);
                    }
                }
            }
        }

        public bool AdvancePlayerProjectile()
        {
            if (playerProjectile != null)
                return playerProjectile.Advance();
            else
                return false;
        }
        public bool AdvanceEnemyProjectile()
        {
            if (enemyProjectile != null)
                return enemyProjectile.Advance();
            else
                return false;
        }
        public bool AdvancePlayerShield()
        {
            if (playerShield != null)
                return playerShield.Advance();
            else
                return false;
        }
        public bool AdvanceEnemyShield()
        {
            if (enemyShield != null)
                return enemyShield.Advance();
            else
                return false;
        }
        public void CreateGhostProjectile(CreateProjectile createProjectile)
        {
            GameObject newGhostEFfect = Instantiate(pathController.ghostProjectilePrefab, GetCoordinatesOnEllipse(projectilePath[1]), Quaternion.identity);
            ghostEffects.Add(newGhostEFfect);
        }
        public void CreateGhostShield(CreateShield createShield)
        {
            GameObject newGhostEFfect = Instantiate(pathController.ghostShieldPrefab, GetCoordinatesOnEllipse(shieldPoint), Quaternion.identity);
            newGhostEFfect.transform.Rotate(new Vector3(0, 0, GetShieldRotation()));
            ghostEffects.Add(newGhostEFfect);
        }
        private float GetShieldRotation()
        {
            Vector2 point1 = GetCoordinatesOnEllipse(shieldPoint);
            float deltaT = 0.0001f;
            if (shieldPoint > 0)
                deltaT *= -1;
            Vector2 point2 = GetCoordinatesOnEllipse(shieldPoint + deltaT);
            Vector2 diff = point2 - point1;
            return Mathf.Atan2(diff.y, diff.x) * 57.296f;
        }
        public void ClearGhostEffects()
        {
            foreach (GameObject obj in ghostEffects)
            {
                Destroy(obj);
            }
            ghostEffects = new List<GameObject>();
        }
        public void CreateProjectile(CreateProjectile createProjectile, int projectilePower, bool isPlayerOwned)
        {
            Vector3 projectilePosition = GetCoordinatesOnEllipse(projectilePath[0]);
            if (!isPlayerOwned)
                projectilePosition.x *= -1;
            Projectile projectile = Instantiate(pathController.projectilePrefab, projectilePosition, Quaternion.identity).GetComponent<Projectile>();
            projectile.strength = createProjectile.strength + projectilePower;
            projectile.element = createProjectile.element;
            projectile.augmentations = createProjectile.augmentations;
            projectile.turnsToArrive = turnsToArrive;
            projectile.target = isPlayerOwned ? pathController.enemyInstance : pathController.playerInstance;
            projectile.path = this;
            projectile.t = projectilePath[0];
            projectile.MovementAnimation();
            if (isPlayerOwned)
                playerProjectile = projectile;
            else
                enemyProjectile = projectile;
        }
        public void CreateShield(CreateShield createShield, int shieldPower, bool isPlayerOwned)
        {
            Vector3 shieldPosition = GetCoordinatesOnEllipse(shieldPoint);
            if (!isPlayerOwned)
                shieldPosition.x *= -1;
            Shield shield = Instantiate(pathController.shieldPrefab, shieldPosition, Quaternion.identity).GetComponent<Shield>();
            float angle = GetShieldRotation();
            if (!isPlayerOwned) angle = (angle * -1) + 180;
            shield.transform.Rotate(new Vector3(0, 0, angle));
            shield.strength = createShield.strength + shieldPower;
            shield.element = createShield.element;
            shield.turnsRemaining = createShield.duration;
            shield.pathName = pathName;
            shield.ownerName = isPlayerOwned ? pathController.playerInstance.characterName : pathController.enemyInstance.characterName;
            if (isPlayerOwned)
                playerShield = shield;
            else
                enemyShield = shield;
        }
        public float GetDestinationT(int movementStep)
        {
            return projectilePath[movementStep + 1];
        }
        public int PredictPlayerShieldNegation(int strength, Element element)
        {
            if (playerShield != null)
                return playerShield.PredictDamageNegated(strength, element);
            else
                return 0;
        }
        public int PredictEnemyShieldEffectiveness(int strength, Element element, int shieldDuration)
        {
            if (playerProjectile == null)
                return 0;
            else
                return playerProjectile.PredictEnemyShieldEffectiveness(strength, element, shieldDuration);
        }
        public int GetNegatedDamage(bool isPlayerProjectile, Projectile projectile)
        {
            if (isPlayerProjectile)
            {
                if (enemyShield != null)
                    return enemyShield.NegateDamage(projectile);
                else
                    return 0;
            }
            else
            {
                if (playerShield != null)
                    return playerShield.NegateDamage(projectile);
                else
                    return 0;
            }
        }
    }
}