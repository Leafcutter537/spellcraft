using System.Collections;
using System.Collections.Generic;
using Assets.Combat.SpellEffects;
using Assets.EventSystem;
using Assets.Inventory.Spells;
using UnityEngine;

namespace Assets.Combat
{
    public class PlayerInstance : CharacterInstance
    {
        [Header("Serialized Object References")]
        [SerializeField] private PlayerStats playerStats;
        [Header("UI References")]
        [SerializeField] private CombatSpellSelectPanel combatSpellSelectPanel;
        [Header("Spellcasting")]
        [SerializeField] private PathSelectEvent pathSelectEvent;
        private bool isReadyForSpell;
        // private GridSquare spellSquare;
        private Vector2Int adjacentVector;
        private int spellDirection;
        private Spell spellBeingCast;
        [Header("Event References")]
        [SerializeField] private StartSpellPreviewEvent startSpellPreviewEvent;
        [SerializeField] private EndSpellPreviewEvent endSpellPreviewEvent;
        [SerializeField] private StartAdjacentVectorPreviewEvent startAdjacentVectorPreviewEvent;
        [SerializeField] private ChooseAdjacentVectorEvent chooseAdjacentVectorEvent;
        [SerializeField] private CastUntargetedSpellEvent castUntargetedSpellEvent;
        // Combat Control
        protected int[] spellCooldowns;

        private void Awake()
        {
            spellCooldowns = new int[10];
            maxHP = playerStats.GetCombatStat(CombatStat.HP);
            maxMP = playerStats.GetCombatStat(CombatStat.MP);
            currentHP = maxHP;
            currentMP = maxMP;
            characterName = playerStats.playerName;
            statPanel.ShowStatInfo();
            baseStats = playerStats.GetStatBundle();
            statusEffects = new List<StatusEffect>();
        }
        private void OnEnable()
        {
            pathSelectEvent.AddListener(OnSquareSelect);
            startSpellPreviewEvent.AddListener(OnStartSpellPreview);
            endSpellPreviewEvent.AddListener(OnEndSpellPreview);
            chooseAdjacentVectorEvent.AddListener(OnChooseAdjacentVector);
            castUntargetedSpellEvent.AddListener(OnCastUntargetedSpell);
        }
        private void OnDisable()
        {
            pathSelectEvent.RemoveListener(OnSquareSelect);
            startSpellPreviewEvent.RemoveListener(OnStartSpellPreview);
            endSpellPreviewEvent.RemoveListener(OnEndSpellPreview);
            chooseAdjacentVectorEvent.RemoveListener(OnChooseAdjacentVector);
            castUntargetedSpellEvent.RemoveListener(OnCastUntargetedSpell);
        }

        public void OnStartSpellPreview(object sender, EventParameters args)
        {
            isReadyForSpell = true;
        }
        public void OnEndSpellPreview(object sender, EventParameters args)
        {
            isReadyForSpell = false;
        }
        private void OnSquareSelect(object sender, EventParameters args)
        {
            if (isReadyForSpell)
            {
                GridSquare spellSquare = sender as GridSquare;
                adjacentVector = new Vector2Int(0, 0);
                spellDirection = 0;
                AttemptCastPlayerSpell(spellSquare);
            }
        }
        private void OnChooseAdjacentVector(object sender, EventParameters args)
        {
            AdjacentChooseEventParameters adjacentArgs = args as AdjacentChooseEventParameters;
            GridSquare spellSquare = adjacentArgs.gridSquare;
            adjacentVector = adjacentArgs.adjacentVector;
            AttemptCastPlayerSpell(spellSquare);
        }
        private void OnCastUntargetedSpell(object sender, EventParameters args)
        {
            spellBeingCast = combatSpellSelectPanel.GetSelected() as Spell;
            CastPlayerSpell(null, spellBeingCast, combatSpellSelectPanel.GetIndex(), Vector2Int.zero, 0);
        }
        public void AttemptCastPlayerSpell(GridSquare square)
        {
            spellBeingCast = combatSpellSelectPanel.GetSelected() as Spell;
            string adjacentEntityName = NeedsAdjacentVector(spellBeingCast);
            if (adjacentEntityName.Length > 0 & adjacentVector == Vector2Int.zero)
            {
                RequestAdjacentVectorChoice(adjacentEntityName, square);
                return;
            }
            if (NeedsDirectionVector(spellBeingCast))
            {
                RequestDirectionChoice();
                return;
            }
            CastPlayerSpell(square, spellBeingCast, combatSpellSelectPanel.GetIndex(), adjacentVector, 0);
            combatSpellSelectPanel.ReturnSpellList();
        }
        private string NeedsAdjacentVector(Spell spell)
        {
            if (spell.targetType == TargetType.Projectile)
            {
                foreach (SpellEffect spellEffect in spell.spellEffects)
                    if (spellEffect is CreateProjectile createProjectile)
                        if (createProjectile.path > 0)
                            return "projectile";
            }
            if (spell.targetType == TargetType.Shield)
            {
                foreach (SpellEffect spellEffect in spell.spellEffects)
                    if (spellEffect is CreateShield createShield)
                        if (createShield.path > 0)
                            return "shield";
            }
            return "";
        }
        private void RequestAdjacentVectorChoice(string adjacentEntityName, GridSquare square)
        {
            startAdjacentVectorPreviewEvent.Raise(this, new AdjacentPreviewEventParameters(square, adjacentEntityName));
        }
        private bool NeedsDirectionVector(Spell spell)
        {
            return false;
        }
        private void RequestDirectionChoice()
        {

        }
        public void CastPlayerSpell(GridSquare square, Spell spell, int spellIndex, Vector2Int adjacentVector, int direction)
        {
            combatLogMessageEvent.Raise(this, new CombatLogEventParameters(GetCombatLogMessage(spell)));
            StatBundle currentStats = GetStatBundle();
            List<ApplyBuff> applyBuffList = new List<ApplyBuff>();
            foreach (SpellEffect spellEffect in spell.spellEffects)
            {
                if (spellEffect is CreateProjectile createProjectile)
                {
                    gridController.CreatePlayerProjectile(square, createProjectile, currentStats.projectilePower, adjacentVector, direction); ;
                }
                if (spellEffect is Heal heal)
                {
                    int totalHeal = heal.strength + currentStats.healPower;
                    currentHP = Mathf.Min(maxHP, currentHP + totalHeal);
                    combatLogMessageEvent.Raise(this, new CombatLogEventParameters(characterName + " healed for " + totalHeal + " HP!"));
                }
                if (spellEffect is CreateShield createShield)
                {
                    gridController.CreatePlayerShield(square, createShield, currentStats.shieldPower, adjacentVector, direction);
                }
                // Buffs are always applied after all other spell effects, as they could affect the others' strength
                // Here we put them in a list to apply them later
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
            SetSpellCooldown(spellIndex, spell.cooldown);
            statPanel.ShowStatInfo();
        }
        public void SetSpellCooldown(int spellIndex, int cooldown)
        {
            spellCooldowns[spellIndex] = cooldown;
        }
        public int GetSpellCooldown(int spellIndex)
        {
            return spellCooldowns[spellIndex];
        }
        public bool CanCastSpell(int manaCost, int spellIndex)
        {
            return (currentMP >= manaCost & spellCooldowns[spellIndex] == 0);
        }
        public void SetStartingCooldowns(List<Spell> spellList)
        {
            for (int i = 0; i < spellList.Count; i++)
            {
                spellCooldowns[i] = spellList[i].chargeTime;
            }
        }

        public bool HasSufficientMana(Spell spell)
        {
            return spell.manaCost <= currentMP;
        }

        public override void EndTurnActions()
        {
            base.EndTurnActions();
            for (int i = 0; i < spellCooldowns.Length; i++)
            {
                spellCooldowns[i] = Mathf.Max(0, spellCooldowns[i] - 1);
            }
        }
    }
}
