using System.Collections;
using System.Collections.Generic;
using Assets.Combat.SpellEffects;
using Assets.EventSystem;
using Assets.Inventory.Spells;
using Assets.Tutorial;
using Unity.VisualScripting;
using UnityEngine;
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
        [SerializeField] private PathSelectEvent pathSelectEvent;
        [SerializeField] private MouseEnterPathEvent mouseEnterPathEvent;
        [SerializeField] private MouseExitPathEvent mouseExitPathEvent;
        [SerializeField] private StartSpellPreviewEvent startSpellPreviewEvent;
        [SerializeField] private EndSpellPreviewEvent endSpellPreviewEvent;
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
        private void Awake()
        {
            combatGrid = new GridSquare[gridSize[0],gridSize[1]];
        }
        private void OnEnable()
        {
            mouseEnterPathEvent.AddListener(OnMouseEnterPath);
            mouseExitPathEvent.AddListener(OnMouseExitPath);
            startSpellPreviewEvent.AddListener(OnStartSpellPreview);
            endSpellPreviewEvent.AddListener(OnEndSpellPreview);
        }
        private void OnDisable()
        {
            mouseEnterPathEvent.RemoveListener(OnMouseEnterPath);
            mouseExitPathEvent.RemoveListener(OnMouseExitPath);
            startSpellPreviewEvent.RemoveListener(OnStartSpellPreview);
            endSpellPreviewEvent.RemoveListener(OnEndSpellPreview);
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
            ClearGhostEffects();
        }
        private void OnMouseEnterPath(object sender, EventParameters args)
        {
            if (isShowingGhost)
            {
                ClearGhostEffects();
                CreateGhostEffects(sender as GridSquare);
            }
        }
        private void OnMouseExitPath(object sender, EventParameters args)
        {
            ClearGhostEffects();
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
                    gridSquare.CreateGhostProjectile(createProjectile);
                }
                if (effect is CreateShield createShield)
                {
                    gridSquare.CreateGhostShield(createShield);
                }
            }
        }
        private void ClearGhostEffects()
        {
            for (int i = 0; i < gridSize[0]; i++)
            {
                for (int j = 0; j < gridSize[1]; j++)
                {
                    combatGrid[i,j].ClearGhostEffects();
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
                    if (combatGrid[x, y].playerProjectile != null & combatGrid[x + 1, y].shield == null)
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
    }
}
