using System.Collections.Generic;
using Assets.Combat.SpellEffects;
using Assets.Inventory.Spells;
using UnityEngine;

namespace Assets.Combat
{
    public abstract class CharacterInstance : MonoBehaviour
    {
        [Header("Controllers")]
        [SerializeField] protected GridController gridController;
        [SerializeField] private TurnController turnController;
        [Header("Combat Log")]
        [SerializeField] protected CombatLogMessageEvent combatLogMessageEvent;
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
            StatBundle stats = GetStatBundle();
            int projectileDamage = Mathf.Max(0, projectile.strength - stats.resilience);
            currentHP = Mathf.Max(0, currentHP - projectileDamage);
            string combatMessage = characterName + " was struck by a projectile for " + projectileDamage + " damage!";
            combatLogMessageEvent.Raise(this, new CombatLogEventParameters(combatMessage));
            foreach (ProjectileAugmentation projectileAugmentation in projectile.augmentations)
            {
                projectileAugmentation.EnactAugmentationEffect(this);
            }
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


        protected void AttemptApplyBuff(ApplyBuff applyBuff)
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
        public void AttemptApplyDebuff(ApplyDebuff applyDebuff)
        {
            StatDebuff existingDebuff = FindExistingDebuff(applyDebuff.stat);
            if (existingDebuff != null)
            {
                if (existingDebuff.debuffStrength >= applyDebuff.strength & existingDebuff.turnsRemaining >= applyDebuff.duration)
                {
                    combatLogMessageEvent.Raise(this, new CombatLogEventParameters(characterName + " already had a stronger " + applyDebuff.stat + " debuff!"));
                    return;
                }
                statusEffects.Remove(existingDebuff);
            }
            statusEffects.Add(new StatDebuff(applyDebuff.strength, applyDebuff.duration, applyDebuff.stat));
            combatLogMessageEvent.Raise(this, new CombatLogEventParameters(characterName + "'s " + PlayerStats.GetCombatStatName(applyDebuff.stat) + " decreased!"));

        }
        public StatDebuff FindExistingDebuff(CombatStat stat)
        {
            foreach (StatusEffect statusEffect in statusEffects)
            {
                if (statusEffect is StatDebuff debuff)
                {
                    if (debuff.stat == stat) return debuff;
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
        protected string GetCombatLogMessage(Spell spell)
        {
            return characterName + " cast " + spell.GetTitle() + "!";
        }

        public virtual void EndTurnActions()
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
                if (statusEffect is StatDebuff statDebuff)
                {
                    switch (statDebuff.stat)
                    {
                        case CombatStat.HP:
                            returnBundle.maxHP -= statDebuff.debuffStrength;
                            break;
                        case CombatStat.MP:
                            returnBundle.maxMP -= statDebuff.debuffStrength;
                            break;
                        case CombatStat.Resilience:
                            returnBundle.resilience -= statDebuff.debuffStrength;
                            break;
                        case CombatStat.ProjectilePower:
                            returnBundle.projectilePower -= statDebuff.debuffStrength;
                            break;
                        case CombatStat.ShieldPower:
                            returnBundle.shieldPower -= statDebuff.debuffStrength;
                            break;
                        case CombatStat.HealPower:
                            returnBundle.healPower -= statDebuff.debuffStrength;
                            break;
                    }
                }
            }
            return returnBundle;
        }

        private void OnMouseEnter()
        {
            if (!gridController.enemyDetailsPanel.gameObject.activeInHierarchy & !gridController.turnController.combatIsEnded & !gridController.tutorialController.isShowingTutorial)
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
