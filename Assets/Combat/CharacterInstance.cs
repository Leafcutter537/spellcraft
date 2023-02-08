using Assets.Combat;
using Assets.Combat.SpellEffects;
using Assets.Inventory.Spells;
using UnityEngine;

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
        foreach (SpellEffect spellEffect in spell.spellEffects)
        {
            if (spellEffect is CreateProjectile createProjectile)
            {
                pathController.CreateProjectile(path, createProjectile, isPlayerOwned);
            }
            if (spellEffect is Heal heal)
            {
                currentHP = Mathf.Min(maxHP, currentHP + heal.strength);
                combatLogMessageEvent.Raise(this, new CombatLogEventParameters(characterName + " healed for " + heal.strength + " HP!"));
            }
            if (spellEffect is CreateShield createShield)
            {
                pathController.CreateShield(path, createShield, isPlayerOwned);
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
}
