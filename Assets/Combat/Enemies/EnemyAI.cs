using System.Collections;
using System.Collections.Generic;
using Assets.Combat.Enemy;
using Assets.Combat.SpellEffects;
using Assets.Inventory.Spells;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Combat
{
    public class EnemyAI : MonoBehaviour
    {
        private List<Spell> spells;
        [SerializeField] private EnemySpellGenerator enemySpellGenerator;
        private int spellIndex;
        [SerializeField] private EnemyInstance enemyInstance;
        [SerializeField] private PathController pathController;
        [SerializeField] private int[] pathPriority;

        public void ResetSpellIndex()
        {
            spellIndex = 0;
        }
        public void SetSpells(List<EnemySpellData> enemySpellData)
        {
            spells = enemySpellGenerator.CreateSpellList(enemySpellData);
        }

        public bool PerformNextSpell()
        {
            while (spellIndex < spells.Count)
                if (AttemptCastSpell())
                    return true;
                else
                    spellIndex++;
            return false;
        }

        private bool AttemptCastSpell()
        {
            switch (spells[spellIndex].targetType)
            {
                case TargetType.Projectile:
                    return AttemptCastPathSpell(spells[spellIndex]);
                case TargetType.Heal:
                    return AttemptCastHealSpell(spells[spellIndex]);
                case TargetType.Shield:
                    return AttemptCastPathSpell(spells[spellIndex]);
                case TargetType.Buff:
                    return AttemptCastBuffSpell(spells[spellIndex]);
            }
            return false;
        }

        private bool AttemptCastBuffSpell(Spell spell)
        {
            foreach (SpellEffect spellEffect in spell.spellEffects)
            {
                if (spellEffect is ApplyBuff applyBuff)
                {
                    foreach (StatusEffect statusEffect in enemyInstance.statusEffects)
                    {
                        if (statusEffect is StatBuff statBuff)
                        {
                            if (applyBuff.stat == statBuff.stat)
                                return false;
                        }
                    }
                }
            }
            enemyInstance.CastSpell(null, spell, false);
            return true;
        }
        private bool AttemptCastHealSpell(Spell spell)
        {
            if (enemyInstance.currentHP >= enemyInstance.maxHP | enemyInstance.currentMP < spell.manaCost)
                return false;
            else
            {
                enemyInstance.CastSpell(null, spell, false);
                return true;
            }
        }
        private bool AttemptCastPathSpell(Spell spell)
        {
            if (enemyInstance.currentMP < spell.manaCost)
            {
                return false;
            }
            List<PathEffectivenessPrediction> pathEffectivenessPredictions = new List<PathEffectivenessPrediction>();
            for (int i = 0; i < pathController.paths.Count; i++)
            {
                int predictedEffectiveness = PredictSpellEffectiveness(spell, pathController.paths[i]);
                predictedEffectiveness = predictedEffectiveness * pathController.paths.Count + pathPriority[i];
                pathEffectivenessPredictions.Add(new PathEffectivenessPrediction(pathController.paths[i], predictedEffectiveness));
            }
            int maxStrength = -1;
            int maxIndex = 0;
            for (int i = 0; i < pathEffectivenessPredictions.Count; i++)
            {
                if (pathEffectivenessPredictions[i].predictedEffectiveness > maxStrength)
                {
                    maxStrength = pathEffectivenessPredictions[i].predictedEffectiveness;
                    maxIndex = i;
                }
            }
            if (maxStrength >= 0)
            {
                enemyInstance.CastSpell(pathEffectivenessPredictions[maxIndex].path, spell, false);
                return true;
            }
            else
            {
                return false;
            }
        }

        private int PredictSpellEffectiveness(Spell spell, Path path)
        {
            if (path.enemyProjectile != null & spell.targetType == TargetType.Projectile)
            {
                return -1;
            }
            if (path.enemyShield != null & spell.targetType == TargetType.Shield)
            {
                return -1;
            }
            int strengthSum = 0;
            foreach (SpellEffect spellEffect in spell.spellEffects)
            {
                if (spellEffect is CreateProjectile createProjectile)
                {
                    Path projectilePath = pathController.GetAdjacentPath(path, createProjectile.path);
                    if (projectilePath == null)
                        continue;
                    if (projectilePath.enemyProjectile == null)
                    {
                        strengthSum += PredictProjectileEffectiveness(createProjectile, path);
                    }
                }
                if (spellEffect is CreateShield createShield)
                {
                    Path projectilePath = pathController.GetAdjacentPath(path, createShield.path);
                    if (projectilePath == null)
                        continue;
                    if (projectilePath.enemyShield == null)
                    {
                        strengthSum += PredictShieldEffectiveness(createShield, path);
                    }
                }
            }
            return strengthSum;
        }
        private int PredictProjectileEffectiveness(CreateProjectile createProjectile, Path path)
        {
            return createProjectile.strength - path.PredictPlayerShieldNegation(createProjectile.strength, createProjectile.element);
        }
        private int PredictShieldEffectiveness(CreateShield createShield, Path path)
        {
            return path.PredictEnemyShieldEffectiveness(createShield.strength, createShield.element, createShield.duration);
        }

    }
}
