using System.Collections;
using System.Collections.Generic;
using Assets.Combat.SpellEffects;
using Assets.EventSystem;
using Assets.Inventory.Spells;
using Assets.Tutorial;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static Assets.Combat.EnemyShieldPattern;

namespace Assets.Combat
{
    public class GridController : MonoBehaviour
    {
        [Header("Turn Controller")]
        public TurnController turnController;
        [Header("Tutorial Controller")]
        public CombatTutorialController tutorialController;
        [Header("Serialized Object References")]
        public EnemySpellGenerator enemySpellGenerator;
        [Header("Grid Dimensions")]
        public GridSquare[,] combatGrid;
        [SerializeField] private Vector2Int gridSize;
        [SerializeField] private Vector2Int currentGridCoordinates;
        public int firstEnemySideColumn;
        [Header("Event References")]
        [SerializeField] private MouseEnterPathEvent mouseEnterPathEvent;
        [SerializeField] private MouseExitPathEvent mouseExitPathEvent;
        [SerializeField] private StartSpellPreviewEvent startSpellPreviewEvent;
        [SerializeField] private EndSpellPreviewEvent endSpellPreviewEvent;
        [SerializeField] private StartAdjacentVectorPreviewEvent startAdjacentVectorPreviewEvent;
        [SerializeField] private ChooseAdjacentVectorEvent chooseAdjacentVectorEvent;
        [Header("UI References")]
        public GameObject enemyDetailsPanel;
        [Header("Combat Spell Panel")]
        public CombatSpellSelectPanel combatSpellSelectPanel;
        [Header("Path Visuals")]
        public Color defaultColor;
        public Color hoverColor;
        [Header("Prefabs")]
        public GameObject projectilePrefab;
        public GameObject shieldPrefab;
        public GameObject ghostProjectilePrefab;
        public GameObject ghostShieldPrefab;
        [Header("Character Instances")]
        public EnemyInstance enemyInstance;
        public PlayerInstance playerInstance;
        // Ghost Effects
        private List<SpellEffect> ghostEffects;
        private bool isShowingGhost;
        private bool isChoosingAdjacentVector;
        // Point System
        private float waitBeforeChoiceTime;
        private PlayerInputActions playerInputActions;
        private InputAction point;
        private InputAction click;
        [SerializeField] new private Camera camera;
        private GridSquare chosenGridSquare;
        private Vector2Int previousAdjacentVector;
        private bool adjacentIsProjectile;
        private bool mouseWithinRadius;
        [SerializeField] private float mouseRadius;
        private void Awake()
        {
            playerInputActions = new PlayerInputActions();
            combatGrid = new GridSquare[gridSize[0],gridSize[1]];
        }
        private void OnEnable()
        {
            point = playerInputActions.UI.Point;
            point.Enable();
            click = playerInputActions.UI.Click;
            click.Enable();
            click.performed += OnClick;
            mouseEnterPathEvent.AddListener(OnMouseEnterPath);
            mouseExitPathEvent.AddListener(OnMouseExitPath);
            startSpellPreviewEvent.AddListener(OnStartSpellPreview);
            endSpellPreviewEvent.AddListener(OnEndSpellPreview);
            startAdjacentVectorPreviewEvent.AddListener(OnStartAdjacentVectorPreviewEvent);
        }
        private void OnDisable()
        {
            point.Disable();
            mouseEnterPathEvent.RemoveListener(OnMouseEnterPath);
            mouseExitPathEvent.RemoveListener(OnMouseExitPath);
            startSpellPreviewEvent.RemoveListener(OnStartSpellPreview);
            endSpellPreviewEvent.RemoveListener(OnEndSpellPreview);
            startAdjacentVectorPreviewEvent.RemoveListener(OnStartAdjacentVectorPreviewEvent);
        }
        private void Update()
        {
            if (isChoosingAdjacentVector)
            {
                waitBeforeChoiceTime += Time.deltaTime;
                Vector2 mousePos = camera.ScreenToWorldPoint(point.ReadValue<Vector2>());
                Vector2 gridSquarePos = new Vector2(chosenGridSquare.transform.position.x, chosenGridSquare.transform.position.y);
                Vector2 relativePosition = mousePos - gridSquarePos;
                if (relativePosition.magnitude > mouseRadius)
                {
                    mouseWithinRadius = false;
                    previousAdjacentVector = Vector2Int.zero;
                    ClearGhostEffects(false);
                    return;
                }
                Vector2Int adjacentVector = GetAdjacentVector(relativePosition);
                if (adjacentVector != previousAdjacentVector)
                {
                    mouseWithinRadius = true;
                    ClearGhostEffects(false);
                    previousAdjacentVector = adjacentVector;
                    Vector2Int coords = GetGridCoordinates(chosenGridSquare);
                    Vector2Int ghostCoords = coords + adjacentVector;
                    if (adjacentIsProjectile)
                        combatGrid[ghostCoords.x, ghostCoords.y].CreateGhostProjectile(null, false);
                    else
                        combatGrid[ghostCoords.x, ghostCoords.y].CreateGhostShield(null, false);
                }
            }
        }
        public void OnStartSpellPreview(object sender, EventParameters args)
        {
            Spell spell = combatSpellSelectPanel.GetSelected() as Spell;
            ghostEffects = spell.spellEffects;
            isShowingGhost = true;
        }
        public void OnEndSpellPreview(object sender, EventParameters args)
        {
            isShowingGhost = false;
            ClearGhostEffects(true);
            isChoosingAdjacentVector = false;
        }
        private void OnMouseEnterPath(object sender, EventParameters args)
        {
            if (isShowingGhost)
            {
                ClearGhostEffects(false);
                CreateGhostEffects(sender as GridSquare);
            }
        }
        private void OnMouseExitPath(object sender, EventParameters args)
        {
            if (!isChoosingAdjacentVector)
                ClearGhostEffects(false);
        }
        private void OnStartAdjacentVectorPreviewEvent(object sender, EventParameters args)
        {
            AdjacentPreviewEventParameters adjArgs = args as AdjacentPreviewEventParameters;
            GridSquare square = adjArgs.square;
            isChoosingAdjacentVector = true;
            chosenGridSquare = square;
            adjacentIsProjectile = adjArgs.entityName == "projectile";
            waitBeforeChoiceTime = 0f;
            if (adjacentIsProjectile)
                square.CreateGhostProjectile(null, true);
            else
                square.CreateGhostShield(null, true);
        }
        private void OnClick(InputAction.CallbackContext context)
        {
            if (isChoosingAdjacentVector & waitBeforeChoiceTime > 0.4f & mouseWithinRadius)
            {
                chooseAdjacentVectorEvent.Raise(this, new AdjacentChooseEventParameters(previousAdjacentVector, chosenGridSquare));
            }
        }
            public void SubscribeGridSquare(GridSquare gridSquare)
        {
            combatGrid[gridSquare.gridPosition[0], gridSquare.gridPosition[1]] = gridSquare;
        }
        private void CreateGhostEffects(GridSquare gridSquare)
        {
            foreach (SpellEffect effect in ghostEffects)
            {
                if (effect is CreateProjectile createProjectile)
                {
                    gridSquare.CreateGhostProjectile(createProjectile, false);
                }
                if (effect is CreateShield createShield)
                {
                    gridSquare.CreateGhostShield(createShield, false);
                }
            }
        }
        private void ClearGhostEffects(bool clearStickies)
        {
            for (int i = 0; i < gridSize[0]; i++)
            {
                for (int j = 0; j < gridSize[1]; j++)
                {
                    combatGrid[i,j].ClearGhostEffects(clearStickies);
                }
            }
        }
        public void CreatePlayerProjectile(GridSquare square, CreateProjectile createProjectile, int projectilePower, Vector2Int adjacentVector, int direction)
        {
            Vector2Int targetCoordinates = GetGridCoordinates(square);
            Vector2Int thisProjectileCoordinates = targetCoordinates + (adjacentVector * createProjectile.path);
            if (thisProjectileCoordinates.x >= 0 & thisProjectileCoordinates.x < gridSize.x &
                thisProjectileCoordinates.y >= 0 & thisProjectileCoordinates.y < gridSize.y)
            {
                combatGrid[thisProjectileCoordinates.x, thisProjectileCoordinates.y].CreateProjectile(createProjectile, projectilePower, true);
            }
        }
        public void CreateEnemyProjectile(EnemyProjectileData projectileData, int projectilePower, bool setPosition)
        {
            Vector2Int projectileCoordinates;
            if (setPosition)
            {
                projectileCoordinates = projectileData.coords;
                projectileCoordinates.x += firstEnemySideColumn;
            }
            else
            {
                projectileCoordinates = new Vector2Int(2, 2);
            }
            if (projectileCoordinates.x >= 0 & projectileCoordinates.x < gridSize.x &
                projectileCoordinates.y >= 0 & projectileCoordinates.y < gridSize.y)
            {
                combatGrid[projectileCoordinates.x, projectileCoordinates.y].CreateEnemyProjectile(projectileData, projectilePower);
            }
        }
        public void CreatePlayerShield(GridSquare square, CreateShield createShield, int shieldPower, Vector2Int adjacentVector, int direction)
        {
            Vector2Int targetCoordinates = GetGridCoordinates(square);
            Vector2Int thisProjectileCoordinates = targetCoordinates + (adjacentVector * createShield.path);
            if (thisProjectileCoordinates.x >= 0 & thisProjectileCoordinates.x < gridSize.x &
                thisProjectileCoordinates.y >= 0 & thisProjectileCoordinates.y < gridSize.y)
            {
                combatGrid[thisProjectileCoordinates.x, thisProjectileCoordinates.y].CreateShield(createShield, shieldPower, true);
            }
        }
        public void CreateEnemyShield(GridSquare square, EnemyShieldData shieldData, int shieldPower, bool createShieldToRight)
        {
            if (createShieldToRight)
            {
                Vector2Int coords = GetGridCoordinates(square);
                coords.x++;
                if (coords.x < gridSize.x)
                    square = combatGrid[coords.x, coords.y];
                else
                    return;
            }
            square.CreateShield(shieldData, shieldPower);
        }
        public List<GridSquare> GetSquaresNeedingShield(ShieldPatternType shieldPatternType)
        {
            List<GridSquare> returnList = new List<GridSquare>();
            List<SquareWithIncomingProjectile> squareList = new List<SquareWithIncomingProjectile>();
            int minColumn = 0;
            int maxColumn = gridSize.x;
            switch (shieldPatternType)
            {
                case ShieldPatternType.BlockFirstColumn:
                    minColumn = firstEnemySideColumn - 1;
                    maxColumn = firstEnemySideColumn - 1;
                    break;
                case ShieldPatternType.BlockSecondColumn:
                    minColumn = firstEnemySideColumn;
                    maxColumn = firstEnemySideColumn;
                    break;
                case ShieldPatternType.BlockBothColumns:
                    minColumn = firstEnemySideColumn - 1;
                    maxColumn = firstEnemySideColumn;
                    break;
            }
            for (int x = minColumn; x <= maxColumn; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    if (combatGrid[x, y].playerProjectile != null & combatGrid[x + 1, y].shield == null & combatGrid[x+1,y].playerProjectile == null)
                        squareList.Add(new SquareWithIncomingProjectile(combatGrid[x, y].playerProjectile.strength, combatGrid[x + 1, y]));
                }
            }
            squareList.Sort();
            foreach (SquareWithIncomingProjectile square in squareList)
            {
                returnList.Add(square.square);
            }
            return returnList;
        }
        public Vector2Int GetGridCoordinates(GridSquare square)
        {
            for (int i = 0; i < gridSize.x; i++)
            {
                for (int j = 0; j < gridSize.y; j++)
                {
                    if (ReferenceEquals(square, combatGrid[i, j]))
                        return new Vector2Int(i, j);
                }
            }
            return new Vector2Int(-1, -1);
        }

        public GridSquare GetNextSquare(GridSquare square, bool isMovingRight)
        {
            Vector2Int coords = GetGridCoordinates(square);
            coords.x += isMovingRight ? 1 : -1;
            if (coords.x < 0 | coords.x >= gridSize.x)
                return null;
            else
                return combatGrid[coords.x, coords.y];
        }
        public void ResetPathIndex()
        {
            currentGridCoordinates = new Vector2Int(-1, -1);
        }
        public bool AdvanceNextPlayerProjectile()
        {
            if (currentGridCoordinates.x == -1 & currentGridCoordinates.y == -1)
            {
                currentGridCoordinates = new Vector2Int(gridSize.x - 1, gridSize.y - 1);
            }
            while (currentGridCoordinates.x >= 0)
            {
                while (currentGridCoordinates.y >= 0)
                {
                    if (combatGrid[currentGridCoordinates.x, currentGridCoordinates.y].AdvancePlayerProjectile())
                    {
                        currentGridCoordinates.y--;
                        return true;
                    }
                    else
                        currentGridCoordinates.y--;
                }
                currentGridCoordinates.y = gridSize.y - 1;
                currentGridCoordinates.x--;
            }
            return false;
        }
        public bool AdvanceNextPlayerShield()
        {
            if (currentGridCoordinates.x == -1 & currentGridCoordinates.y == -1)
            {
                currentGridCoordinates = new Vector2Int(firstEnemySideColumn - 1, gridSize.y - 1);
            }
            while (currentGridCoordinates.x >= 0)
            {
                while (currentGridCoordinates.y >= 0)
                {
                    if (combatGrid[currentGridCoordinates.x, currentGridCoordinates.y].AdvanceShield())
                    {
                        currentGridCoordinates.y--;
                        return true;
                    }
                    else
                        currentGridCoordinates.y--;
                }
                currentGridCoordinates.y = gridSize.y - 1;
                currentGridCoordinates.x--;
            }
            return false;
        }
        public bool AdvanceNextEnemyProjectile()
        {
            if (currentGridCoordinates.x == -1 & currentGridCoordinates.y == -1)
            {
                currentGridCoordinates = new Vector2Int(0, gridSize.y - 1);
            }
            while (currentGridCoordinates.x < gridSize.x)
            {
                while (currentGridCoordinates.y >= 0)
                {
                    if (combatGrid[currentGridCoordinates.x, currentGridCoordinates.y].AdvanceEnemyProjectile())
                    {
                        currentGridCoordinates.y--;
                        return true;
                    }
                    else
                        currentGridCoordinates.y--;
                }
                currentGridCoordinates.y = gridSize.y - 1;
                currentGridCoordinates.x++;
            }
            return false;
        }

        public bool IsPlayerSideGridSquare(GridSquare square)
        {
            Vector2Int coords = GetGridCoordinates(square);
            return coords.x < firstEnemySideColumn;
        }
        public bool AdvanceNextEnemyShield()
        {
            if (currentGridCoordinates.x == -1 & currentGridCoordinates.y == -1)
            {
                currentGridCoordinates = new Vector2Int(firstEnemySideColumn, gridSize.y - 1);
            }
            while (currentGridCoordinates.x < gridSize.x)
            {
                while (currentGridCoordinates.y >= 0)
                {
                    if (combatGrid[currentGridCoordinates.x, currentGridCoordinates.y].AdvanceShield())
                    {
                        currentGridCoordinates.y--;
                        return true;
                    }
                    else
                        currentGridCoordinates.y--;
                }
                currentGridCoordinates.y = gridSize.y - 1;
                currentGridCoordinates.x++;
            }
            return false;
        }

        public bool SquaresAreHoverable()
        {
            if (enemyDetailsPanel.gameObject.activeInHierarchy)
                return false;
            if (turnController.combatIsEnded)
                return false;
            if (tutorialController.isShowingTutorial)
                return false;
            if (isChoosingAdjacentVector)
                return false;
            return true;
        }

        public bool IsEdgeSquare(GridSquare square)
        {
            Vector2Int coords = GetGridCoordinates(square);
            return coords.x == 0 | (coords.x+1) == gridSize.x;
        }

        private Vector2Int GetAdjacentVector(Vector2 relativePosition)
        {
            List<Vector2Int> possibleVectors = new List<Vector2Int>();
            Vector2Int currentCoords = GetGridCoordinates(chosenGridSquare);
            if (currentCoords.x > 0)
                possibleVectors.Add(new Vector2Int(-1, 0));
            if (currentCoords.y > 0)
                possibleVectors.Add(new Vector2Int(0, -1));
            if (currentCoords.x < firstEnemySideColumn - 1)
                possibleVectors.Add(new Vector2Int(1, 0));
            if (currentCoords.y < gridSize.y - 1)
                possibleVectors.Add(new Vector2Int(0, 1));
            float smallestAngle = 180;
            Vector2Int chosenVector = possibleVectors[0];
            for (int i = 0; i < possibleVectors.Count; i++)
            {
                float angleBetween = Vector2.Angle(relativePosition, possibleVectors[i]);
                if (angleBetween < smallestAngle)
                {
                    chosenVector = possibleVectors[i];
                    smallestAngle = angleBetween;
                }
            }
            return chosenVector;
        }
    }
}
