using System.Collections.Generic;
using Assets.Combat.SpellEffects;
using Assets.EventSystem;
using Assets.Inventory.Spells;
using Assets.Tutorial;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Combat
{
    public class PathController : MonoBehaviour
    {
        [Header("Turn Controller")]
        public TurnController turnController;
        [Header("Tutorial Controller")]
        public CombatTutorialController tutorialController;
        [Header("Paths")]
        public List<Path> paths;
        private int pathIndex;
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
        private void OnMouseEnterPath(object sender, EventParameters args)
        {
            if (isShowingGhost)
            {
                ClearGhostEffects();
                CreateGhostEffects(sender as Path);
            }
        }
        private void OnMouseExitPath(object sender, EventParameters args)
        {
            ClearGhostEffects();
        }
        public int GetPathIndex(Path path)
        {
            for (int i = 0; i < paths.Count; i++)
            {
                if (ReferenceEquals(paths[i], path))
                    return i;
            }
            return -1;
        }
        public Path GetAdjacentPath(Path path, int offset)
        {
            int pathIndex = GetPathIndex(path);
            int returnIndex = pathIndex + offset;
            if (returnIndex >= 0 & returnIndex < paths.Count)
            {
                return paths[returnIndex];
            }
            else
            {
                return null;
            }
        }
        public void ResetPathIndex()
        {
            pathIndex = 0;
        }
        public bool AdvanceNextPlayerProjectile()
        {
            while (pathIndex < paths.Count)
            {
                if (paths[pathIndex].AdvancePlayerProjectile())
                    return true;
                else
                    pathIndex++;
            }
            return false;
        }
        public bool AdvanceNextEnemyProjectile()
        {
            while (pathIndex < paths.Count)
            {
                if (paths[pathIndex].AdvanceEnemyProjectile())
                    return true;
                else
                    pathIndex++;
            }
            return false;
        }
        public bool AdvanceNextPlayerShield()
        {
            while (pathIndex < paths.Count)
            {
                if (paths[pathIndex].AdvancePlayerShield())
                    return true;
                else
                    pathIndex++;
            }
            return false;
        }
        public bool AdvanceNextEnemyShield()
        {
            while (pathIndex < paths.Count)
            {
                if (paths[pathIndex].AdvanceEnemyShield())
                    return true;
                else
                    pathIndex++;
            }
            return false;
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
        private void CreateGhostEffects(Path path)
        {
            int selectedPathIndex = GetPathIndex(path);
            foreach (SpellEffect effect in ghostEffects)
            {
                if (effect is CreateProjectile createProjectile)
                {
                    int ghostIndex = selectedPathIndex + createProjectile.path;
                    if (ghostIndex >= 0 & ghostIndex < paths.Count)
                    {
                        paths[ghostIndex].CreateGhostProjectile(createProjectile);
                    }
                }
                if (effect is CreateShield createShield)
                {
                    int ghostIndex = selectedPathIndex + createShield.path;
                    if (ghostIndex >= 0 & ghostIndex < paths.Count)
                    {
                        paths[ghostIndex].CreateGhostShield(createShield);
                    }
                }
            }
        }
        public void CreateProjectile(Path path, CreateProjectile createProjectile, int projectilePower, bool isPlayerOwned)
        {
            int targetIndex = GetPathIndex(path);
            int thisProjectileIndex = targetIndex + createProjectile.path;
            if (thisProjectileIndex >= 0 & thisProjectileIndex < paths.Count)
            {
                paths[thisProjectileIndex].CreateProjectile(createProjectile, projectilePower, isPlayerOwned);
            }
        }
        public void CreateShield(Path path, CreateShield createShield, int shieldPower, bool isPlayerOwned)
        {
            int targetIndex = GetPathIndex(path);
            int thisShieldIndex = targetIndex + createShield.path;
            if (thisShieldIndex >= 0 & thisShieldIndex < paths.Count)
            {
                paths[thisShieldIndex].CreateShield(createShield, shieldPower, isPlayerOwned);
            }
        }
        private void ClearGhostEffects()
        {
            foreach (Path path in paths)
            {
                path.ClearGhostEffects();
            }
        }

    }
}

