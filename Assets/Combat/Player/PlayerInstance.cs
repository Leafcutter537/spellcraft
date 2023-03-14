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
        [Header("Event References")]
        [SerializeField] private StartSpellPreviewEvent startSpellPreviewEvent;
        [SerializeField] private EndSpellPreviewEvent endSpellPreviewEvent;

        private void Awake()
        {
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
            pathSelectEvent.AddListener(OnPathSelect);
            startSpellPreviewEvent.AddListener(OnStartSpellPreview);
            endSpellPreviewEvent.AddListener(OnEndSpellPreview);
        }
        private void OnDisable()
        {
            pathSelectEvent.RemoveListener(OnPathSelect);
            startSpellPreviewEvent.RemoveListener(OnStartSpellPreview);
            endSpellPreviewEvent.RemoveListener(OnEndSpellPreview);
        }

        public void OnStartSpellPreview(object sender, EventParameters args)
        {
            isReadyForSpell = true;
        }
        public void OnEndSpellPreview(object sender, EventParameters args)
        {
            isReadyForSpell = false;
        }
        private void OnPathSelect(object sender, EventParameters args)
        {
            if (isReadyForSpell)
            {
                CastSpell(sender as Path);
            }
        }
        public void CastSpell(Path path)
        {
            Spell spell = combatSpellSelectPanel.GetSelected() as Spell;
            CastSpell(path, spell, true);
            combatSpellSelectPanel.ReturnSpellList();
        }


        public bool HasSufficientMana(Spell spell)
        {
            return spell.manaCost <= currentMP;
        }
    }
}
