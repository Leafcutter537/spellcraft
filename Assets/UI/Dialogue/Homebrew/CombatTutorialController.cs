using System.Collections;
using System.Collections.Generic;
using Assets.EventSystem;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Tutorial
{
    public class CombatTutorialController : TutorialController
    {
        [Header("UI References")]
        [SerializeField] private Button endTurnButton;
        [Header("Event References")]
        [SerializeField] private StartSpellPreviewEvent startSpellPreviewEvent;
        [SerializeField] private PathSelectEvent pathSelectEvent;
        [Header("Line Text")]
        [SerializeField] private TutorialLineDatabase tutorialLineDatabase;
        // Sequencing
        private bool hasShownSpellPreview;
        private int spellsCastCount;
        private bool hasStartedTurn;

        private void Start()
        {
            if (QuestLog.IsQuestActive("Complete First Trial"))
            {
                endTurnButton.interactable = false;
                SetupDialogue(tutorialLineDatabase.firstCombatStartLines) ;
            }
            else if (QuestLog.IsQuestActive("Complete Second Trial"))
            {
                SetupDialogue(tutorialLineDatabase.secondCombatStartLines);
            }
            else if (QuestLog.IsQuestActive("Complete Third Trial"))
            {
                SetupDialogue(tutorialLineDatabase.thirdCombatStartLines);
            }
        }
        private void OnEnable()
        {
            startSpellPreviewEvent.AddListener(OnStartSpellPreview);
            pathSelectEvent.AddListener(OnCastSpell);
        }
        private void OnDisable()
        {
            startSpellPreviewEvent.RemoveListener(OnStartSpellPreview);
            pathSelectEvent.RemoveListener(OnCastSpell);
        }

        private void OnStartSpellPreview(object sender, EventParameters args)
        {
            if (!hasShownSpellPreview & QuestLog.IsQuestActive("Complete First Trial"))
            {
                activeLines = tutorialLineDatabase.firstCombatSelectSpell;
                InitiateDialogue();
            }
            hasShownSpellPreview = true;
        }
        private void OnCastSpell(object sender, EventParameters args)
        {
            if (spellsCastCount == 0 & QuestLog.IsQuestActive("Complete First Trial"))
            {
                activeLines = tutorialLineDatabase.firstCombatSelectPath;
                InitiateDialogue();
            }
            if (spellsCastCount == 2 & QuestLog.IsQuestActive("Complete First Trial"))
            {
                activeLines = tutorialLineDatabase.firstCombatCastThirdSpell;
                endTurnButton.interactable = true;
                InitiateDialogue();
            }
            spellsCastCount++;
        }

        public void OnStartPlayerTurn()
        {
            if (!hasStartedTurn & QuestLog.IsQuestActive("Complete First Trial"))
            {
                activeLines = tutorialLineDatabase.firstCombatStartSecondPlayerTurn;
                InitiateDialogue();
            }
            if (!hasStartedTurn & QuestLog.IsQuestActive("Complete Third Trial"))
            {
                activeLines = tutorialLineDatabase.thirdCombatStartSecondPlayerTurn;
                InitiateDialogue();
            }
            hasStartedTurn = true;
        }
    }
}
