using System.Collections.Generic;
using Assets.Combat.SpellEffects;
using Assets.Inventory.Spells;
using UnityEngine;

namespace Assets.Combat
{
    public abstract class CharacterInstance : MonoBehaviour
    {
        [Header("Controllers")]
        [SerializeField] private PathController pathController;
        [SerializeField] private TurnController turnController;
        [Header("Combat Log")]
        [SerializeField] private CombatLogMessageEvent combatLogMessageEvent;
        [Header("Stat Panel")]
        [SerializeField] protected StatPanel statPanel;
        [HideInInspector] public string characterName;
        [HideInInspector] public int currentHP;
        [HideInInspector] public int currentMP;
        [HideInInspector] public int maxHP;
        [HideInInspector] public int maxMP;
        [Header("Event References")]
        [SerializeField] private EnterTooltipEvent enterTooltipEvent;
        [SerializeField] private ExitTooltipEvent exitTooltipEvent;
        [Header("Visuals")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Color defaultColor;
        [SerializeField] private Color hoverColor;
        // Stat and Buffs
        protected StatBundle baseStats;
        public List<StatusEffect> statusEffects;
        private List<StatusEffect> expiredStatusEffect;
        private int statusEffectIndex;
        private bool isLoopingThroughStatusEffects;

        public void ReceiveProjectile(Projectile projectile)
        {
            currentHP = Mathf.Max(0, currentHP - projectile.strength);
            string combatMessage = characterName + " was struck by a projectile for " + projectile.strength + " damage!";
            combatLogMessageEvent.Raise(this, new CombatLogEventParameters(combatMessage));
            statPanel.ShowStatInfo();
            CheckDeath();
        }

        private void CheckDeath()
        {
            if (currentHP <= 0)
            {
                turnController.EndCombat(this is EnemyInstance);
            }
        }

        public void CastSpell(Path path, Spell spell, bool isPlayerOwned)
        {
            combatLogMessageEvent.Raise(this, new CombatLogEventParameters(GetCombatLogMessage(spell, path)));
            StatBundle currentStats = GetStatBundle();
            List<ApplyBuff> applyBuffList = new List<ApplyBuff>();
            foreach (SpellEffect spellEffect in spell.spellEffects)
            {
                if (spellEffect is CreateProjectile createProjectile)
                {
                    pathController.CreateProjectile(path, createProjectile, currentStats.projectilePower, isPlayerOwned); ;
                }
                if (spellEffect is Heal heal)
                {
                    int totalHeal = heal.strength + currentStats.healPower;
                    currentHP = Mathf.Min(maxHP, currentHP + totalHeal);
                    combatLogMessageEvent.Raise(this, new CombatLogEventParameters(characterName + " healed for " + totalHeal + " HP!"));
                }
                if (spellEffect is CreateShield createShield)
                {
                    pathController.CreateShield(path, createShield, currentStats.projectilePower, isPlayerOwned);
                }
                // Buffs are always applied after all other spell effects, as they could affect the others' strength
                if (spellEffect is ApplyBuff applyBuff)
                {
                    applyBuffList.Add(applyBuff);
                }
            }
            foreach (ApplyBuff applyBuff in applyBuffList)
            {
                AttemptApplyBuff(applyBuff);
            }
            currentMP -= spell.manaCost;
            statPanel.ShowStatInfo();
        }
        private void AttemptApplyBuff(ApplyBuff applyBuff)
        {
            StatBuff existingBuff = FindExistingBuff(applyBuff.stat);
            if (existingBuff != null)
            {
                if (existingBuff.buffStrength >= applyBuff.buffStrength & existingBuff.turnsRemaining >= applyBuff.duration)
                {
                    combatLogMessageEvent.Raise(this, new CombatLogEventParameters(characterName + " already had a stronger " + applyBuff.stat + " buff!"));
                    return;
                }
                statusEffects.Remove(existingBuff);
            }
            statusEffects.Add(new StatBuff(applyBuff.buffStrength, applyBuff.duration, applyBuff.stat));
            combatLogMessageEvent.Raise(this, new CombatLogEventParameters(characterName + "'s " + PlayerStats.GetCombatStatName(applyBuff.stat) + " increased!"));
        }
        public StatBuff FindExistingBuff(CombatStat stat)
        {
            foreach (StatusEffect statusEffect in statusEffects)
            {
                if (statusEffect is StatBuff buff)
                {
                    if (buff.stat == stat) return buff;
                }
            }
            return null;
        }
        public bool AdvanceNextStatusEffect()
        {
            if (isLoopingThroughStatusEffects == false)
            {
                statusEffectIndex = 0;
                isLoopingThroughStatusEffects = true;
                expiredStatusEffect = new List<StatusEffect>();
            }
            while (statusEffectIndex < statusEffects.Count)
            {
                statusEffects[statusEffectIndex].turnsRemaining--;
                if (statusEffects[statusEffectIndex].turnsRemaining <= 0)
                {
                    string logMessage = characterName + "'s " + statusEffects[statusEffectIndex].GetTitle() + " expired!";
                    combatLogMessageEvent.Raise(this, new CombatLogEventParameters(logMessage));
                    expiredStatusEffect.Add(statusEffects[statusEffectIndex]);
                    statusEffectIndex++;
                    return true;
                }
                else
                    statusEffectIndex++;
            }
            foreach (StatusEffect expiredStatusEffect in expiredStatusEffect)
            {
                statusEffects.Remove(expiredStatusEffect);
            }
            isLoopingThroughStatusEffects = false;
            return false;
        }
        protected string GetCombatLogMessage(Spell spell, Path path)
        {
            if (path != null)
                return characterName + " cast " + spell.GetTitle() + " along the " + path.pathName + "!";
            else
                return characterName + " cast " + spell.GetTitle() + "!";
        }

        public void RestoreMana()
        {
            currentMP = maxMP;
            statPanel.ShowStatInfo();
        }

        public StatBundle GetStatBundle()
        {
            StatBundle returnBundle = new StatBundle(baseStats);
            foreach (StatusEffect statusEffect in statusEffects)
            {
                if (statusEffect is StatBuff statBuff)
                {
                    switch (statBuff.stat)
                    {
                        case CombatStat.HP:
                            returnBundle.maxHP += statBuff.buffStrength;
                            break;
                        case CombatStat.MP:
                            returnBundle.maxMP += statBuff.buffStrength;
                            break;
                        case CombatStat.Resilience:
                            returnBundle.resilience += statBuff.buffStrength;
                            break;
                        case CombatStat.ProjectilePower:
                            returnBundle.projectilePower += statBuff.buffStrength;
                            break;
                        case CombatStat.ShieldPower:
                            returnBundle.shieldPower += statBuff.buffStrength;
                            break;
                        case CombatStat.HealPower:
                            returnBundle.healPower += statBuff.buffStrength;
                            break;
                    }
                }
            }
            return returnBundle;
        }

        private void OnMouseEnter()
        {
            if (!pathController.enemyDetailsPanel.gameObject.activeInHierarchy & !pathController.turnController.combatIsEnded & !pathController.tutorialController.isShowingTutorial)
            {
                spriteRenderer.color = hoverColor;
                enterTooltipEvent.Raise(this, null);
            }
        }

        private void OnMouseExit()
        {
            spriteRenderer.color = defaultColor;
            exitTooltipEvent.Raise(this, null);
        }
    }
}
