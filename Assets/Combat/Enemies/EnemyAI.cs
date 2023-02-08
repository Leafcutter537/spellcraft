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
                    return AttemptCastProjectileSpell(spells[spellIndex]);
                case TargetType.Heal:
                    return AttemptCastHealSpell(spells[spellIndex]);
            }
            return false;
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
        private bool AttemptCastProjectileSpell(Spell spell)
        {
            if (enemyInstance.currentMP < spell.manaCost)
            {
                return false;
            }
            List<PathDamagePrediction> pathDamagePredictions = new List<PathDamagePrediction>();
            for (int i = 0; i < pathController.paths.Count; i++)
            {
                int predictedDamage = PredictSpellDamage(spell, pathController.paths[i]);
                predictedDamage = predictedDamage * pathController.paths.Count + pathPriority[i];
                pathDamagePredictions.Add(new PathDamagePrediction(pathController.paths[i], predictedDamage));
            }
            int maxDamage = 0;
            int maxIndex = 0;
            for (int i = 0; i < pathDamagePredictions.Count; i++)
            {
                if (pathDamagePredictions[i].predictedDamage > maxDamage)
                {
                    maxDamage = pathDamagePredictions[i].predictedDamage;
                    maxIndex = i;
                }
            }
            if (maxDamage > 0)
            {
                enemyInstance.CastSpell(pathDamagePredictions[maxIndex].path, spell, false);
                return true;
            }
            else
            {
                return false;
            }
        }
        private int PredictSpellDamage(Spell spell, Path path)
        {
            if (path.enemyProjectile != null)
            {
                return -1;
            }
            int damageSum = 0;
            foreach (SpellEffect spellEffect in spell.spellEffects)
            {
                if (spellEffect is CreateProjectile createProjectile)
                {
                    Path projectilePath = pathController.GetAdjacentPath(path, createProjectile.path);
                    if (projectilePath.enemyProjectile == null)
                    {
                        damageSum += createProjectile.strength - projectilePath.PredictPlayerShieldNegation(createProjectile.strength, createProjectile.element);
                    }
                }
            }
            return damageSum;
        }
    }
}
