using System.Collections;
using System.Collections.Generic;
using Assets.Combat;
using Assets.Combat.SpellEffects;
using Assets.Inventory.Spells;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Combat
{
    public class GridSquare : MonoBehaviour
    {
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
        [SerializeField] private GridController gridController;
        [Header("Grid position")]
        public int[] gridPosition;
        [Header("Projectile")]
        [HideInInspector] public Projectile playerProjectile;
        [HideInInspector] public Projectile enemyProjectile;
        [Header("Shields")]
        [HideInInspector] public Shield shield;
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
        private void Start()
        {
            gridController.SubscribeGridSquare(this);
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
        private void OnMouseEnter()
        {
            if (!gridController.enemyDetailsPanel.gameObject.activeInHierarchy & !gridController.turnController.combatIsEnded & !gridController.tutorialController.isShowingTutorial)
            {
                isHovering = true;
                spriteRenderer.color = gridController.hoverColor;
                mouseEnterPathEvent.Raise(this, null);
                enterTooltipEvent.Raise(this, null);
            }
        }

        private void OnMouseExit()
        {
            isHovering = false;
            spriteRenderer.color = gridController.defaultColor;
            mouseExitPathEvent.Raise(this, null);
            exitTooltipEvent.Raise(this, null);
        }

        private void OnClick(InputAction.CallbackContext context)
        {
            if (isHovering)
            {
                Spell spell = gridController.combatSpellSelectPanel.GetSelected() as Spell;
                if (spell != null)
                {
                    if ((spell.targetType == TargetType.Shield | spell.targetType == TargetType.Projectile) & !gridController.IsPlayerSideGridSquare(this))
                    {
                        tooltipWarningEvent.Raise(this, new TooltipWarningEventParameters("You may only create spells on your side of the combat space!"));
                    }
                    else if ((spell.targetType == TargetType.Shield | spell.targetType == TargetType.Projectile) & playerProjectile != null)
                    {
                        tooltipWarningEvent.Raise(this, new TooltipWarningEventParameters("You already have a projectile in that space!"));
                    }
                    else if ((spell.targetType == TargetType.Shield | spell.targetType == TargetType.Projectile) & shield != null)
                    {
                        tooltipWarningEvent.Raise(this, new TooltipWarningEventParameters("You already have a shield in that space!"));
                    }
                    else
                    {
                        pathSelectEvent.Raise(this, null);
                    }
                }
            }
        }

        public void CreateGhostProjectile(CreateProjectile createProjectile)
        {
            GameObject newGhostEFfect = Instantiate(gridController.ghostProjectilePrefab, transform.position, Quaternion.identity);
            ghostEffects.Add(newGhostEFfect);
        }
        public void CreateGhostShield(CreateShield createShield)
        {
            GameObject newGhostEFfect = Instantiate(gridController.ghostShieldPrefab, transform.position, Quaternion.identity);
            ghostEffects.Add(newGhostEFfect);
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
            Projectile projectile = Instantiate(gridController.projectilePrefab, transform.position, Quaternion.identity).GetComponent<Projectile>();
            projectile.strength = createProjectile.strength + projectilePower;
            projectile.element = createProjectile.element;
            projectile.augmentations = createProjectile.augmentations;
            projectile.target = isPlayerOwned ? gridController.enemyInstance : gridController.playerInstance;
            projectile.square = this;
            projectile.gridController = gridController;
            if (isPlayerOwned)
                playerProjectile = projectile;
            else
                enemyProjectile = projectile;
        }
        public void CreateEnemyProjectile(EnemyProjectileData projectileData, int projectilePower)
        {
            Projectile projectile = Instantiate(gridController.projectilePrefab, transform.position, Quaternion.identity).GetComponent<Projectile>();
            projectile.strength = projectileData.strength + projectilePower;
            projectile.element = projectileData.element;
            projectile.augmentations = gridController.enemySpellGenerator.CreateProjectileAugmentations(projectileData.augmentations);
            projectile.target = gridController.playerInstance;
            projectile.square = this;
            projectile.gridController = gridController;
            enemyProjectile = projectile;
        }
        public void CreateShield(CreateShield createShield, int shieldPower, bool isPlayerOwned)
        {
            shield = Instantiate(gridController.shieldPrefab, transform.position, Quaternion.identity).GetComponent<Shield>();
            shield.strength = createShield.strength + shieldPower;
            shield.element = createShield.element;
            shield.turnsRemaining = createShield.duration;
            shield.ownerName = isPlayerOwned ? gridController.playerInstance.characterName : gridController.enemyInstance.characterName;
        }
        public void CreateShield(EnemyShieldData shieldData, int shieldPower)
        {
            shield = Instantiate(gridController.shieldPrefab, transform.position, Quaternion.identity).GetComponent<Shield>();
            shield.strength = shieldData.strength + shieldPower;
            shield.element = shieldData.element;
            shield.turnsRemaining = shieldData.duration;
            shield.ownerName = gridController.enemyInstance.characterName;
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
        public bool AdvanceShield()
        {
            if (shield != null)
                return shield.Advance();
            else
                return false;
        }

    }
}
