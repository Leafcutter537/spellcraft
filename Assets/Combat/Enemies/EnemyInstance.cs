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
            spellCooldowns = new int[10];
            characterName = currentEnemy.enemyStats.enemyName;
            enemyStats = currentEnemy.enemyStats;
            currentHP = enemyStats.maxHP;
            currentMP = enemyStats.maxMP;
            maxHP = enemyStats.maxHP;
            maxMP = enemyStats.maxMP;
            enemyID = enemyStats.enemyID;
            baseStats = enemyStats.GetStatBundle();
            enemyAI.SetSpells(currentEnemy.enemyStats.spells);
            SetStartingCooldowns(enemyAI.spells);
            statPanel.ShowStatInfo();
            statusEffects = new List<StatusEffect>();
        }

        public void StartTurn()
        {
            enemyAI.ResetSpellIndex();
        }
        public bool PerformNextSpell()
        {
            return enemyAI.PerformNextSpell();
        }
    }
}
