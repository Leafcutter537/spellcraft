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
        protected StatBundle baseStats;

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
            }
            currentMP -= spell.manaCost;
            statPanel.ShowStatInfo();
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
            return baseStats;
        }
    }
}
