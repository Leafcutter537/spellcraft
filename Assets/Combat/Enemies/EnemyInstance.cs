using System.Collections;
using System.Collections.Generic;
using Assets.Combat.SpellEffects;
using UnityEngine;


namespace Assets.Combat
{
    public class EnemyInstance : CharacterInstance
    {
        [SerializeField] private CurrentEnemy currentEnemy;
        public EnemyStats enemyStats;
        public int enemyID;
        [SerializeField] private EnemyAI enemyAI;

        private void Awake()
        {
            characterName = currentEnemy.enemyStats.enemyName;
            enemyStats = currentEnemy.enemyStats;
            currentHP = enemyStats.maxHP;
            currentMP = enemyStats.maxMP;
            maxHP = enemyStats.maxHP;
            maxMP = enemyStats.maxMP;
            enemyID = enemyStats.enemyID;
            baseStats = enemyStats.GetStatBundle();
            enemyAI.SetPatterns(currentEnemy.enemyStats.projectilePatterns,
                currentEnemy.enemyStats.shieldPatterns, 
                currentEnemy.enemyStats.healPatterns,
                currentEnemy.enemyStats.buffPatterns);
            statPanel.ShowStatInfo();
            statusEffects = new List<StatusEffect>();
        }


        public bool PerformNextSpell()
        {
            return enemyAI.PerformNextSpell();
        }
        public override void EndTurnActions()
        {
            base.EndTurnActions();
            enemyAI.ResetSpellcasting();
        }

        public bool CastProjectileSpell(EnemyProjectilePattern spell)
        {
            OnSuccessfulCast(spell);
            StatBundle currentStats = GetStatBundle();
            foreach (EnemyProjectileData projectileData in spell.projectileData)
            {
                gridController.CreateEnemyProjectile(projectileData, currentStats.projectilePower, spell.projectilePatternType == EnemyProjectilePattern.ProjectilePatternType.SetPosition);
            }
            return true;
        }

        public bool CastShieldSpell(EnemyShieldPattern spell)
        {
            List<GridSquare> targetSquares = new List<GridSquare>();
            bool fixedTargets;
            if (spell.shieldPatternType == EnemyShieldPattern.ShieldPatternType.BlockFirstColumn |
                spell.shieldPatternType == EnemyShieldPattern.ShieldPatternType.BlockSecondColumn |
                spell.shieldPatternType == EnemyShieldPattern.ShieldPatternType.BlockBothColumns)
            {
                fixedTargets = true;
                targetSquares = gridController.GetSquaresNeedingShield(spell.shieldPatternType);
                if (targetSquares.Count == 0)
                    return false;
            }
            else
            {
                fixedTargets = false;
                foreach (EnemyShieldData shieldData in spell.shieldData)
                {
                    Vector2Int coords = shieldData.coords;
                    coords.x += gridController.firstEnemySideColumn;
                    targetSquares.Add(gridController.combatGrid[coords.x,coords.y]);
                }
            }
            OnSuccessfulCast(spell);
            StatBundle currentStats = GetStatBundle();
            int i = 0;
            while (i < targetSquares.Count & i < spell.shieldData.Count)
            {
                bool createShieldToRight = !fixedTargets & spell.shieldData[i].coords == new Vector2Int(1, 0);
                gridController.CreateEnemyShield(targetSquares[i], spell.shieldData[i], currentStats.shieldPower, createShieldToRight);
                i++;
            }
            return true;
        }

        public bool CastHealSpell(EnemyHealPattern spell)
        {
            if (currentHP >= maxHP)
                return false;
            OnSuccessfulCast(spell);
            StatBundle currentStats = GetStatBundle();
            int totalHeal = spell.strength + currentStats.healPower;
            currentHP = Mathf.Min(maxHP, currentHP + totalHeal);
            combatLogMessageEvent.Raise(this, new CombatLogEventParameters(characterName + " healed for " + totalHeal + " HP!"));
            return true;
        }

        public bool CastBuffSpell(EnemyBuffPattern spell)
        {
            OnSuccessfulCast(spell);
            foreach (EnemyBuffData buffData in spell.enemyBuffData)
            {
                ApplyBuff applyBuff = new ApplyBuff(buffData.buffStrength, buffData.buffDuration, buffData.combatStat);
                AttemptApplyBuff(applyBuff);
            }
            return true;
        }

        public bool CastSpell(EnemySpellPattern spell)
        {
            if (spell is EnemyProjectilePattern enemyProjectilePattern)
                return CastProjectileSpell(enemyProjectilePattern);
            if (spell is EnemyShieldPattern enemyShieldPattern)
                return CastShieldSpell(enemyShieldPattern);
            if (spell is EnemyHealPattern enemyHealPattern)
                return CastHealSpell(enemyHealPattern);
            if (spell is EnemyBuffPattern enemyBuffPattern)
                return CastBuffSpell(enemyBuffPattern);
            return false;
        }

        private void OnSuccessfulCast(EnemySpellPattern spell)
        {
            combatLogMessageEvent.Raise(this, new CombatLogEventParameters(characterName + " cast " + spell.spellName + "!"));
            spell.SetCastCooldown();
        }
    }
}
