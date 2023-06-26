using System.Collections;
using System.Collections.Generic;
using Assets.Inventory.Runes;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Assets.Tutorial
{
    public class SpellforgeTutorialController : TutorialController
    {
        [SerializeField] private TutorialLineDatabase tutorialLineDatabase;
        [SerializeField] private ScrollDisplay scrollDisplay;
        private void Start()
        {
            if (QuestLog.IsQuestActive("Create a Shield Spell"))
            {
                SetupDialogue(tutorialLineDatabase.createFirstSpell);
            }
        }

        public bool SpellIsCorrectDuringTutorial()
        {
            if (!QuestLog.IsQuestActive("Create a Shield Spell"))
            {
                return true;
            }
            List<Rune> runesUsedInSpell = new List<Rune>();
            foreach (RuneSelectPanelChoice runeSlot in scrollDisplay.runeSlots)
            {
                runesUsedInSpell.Add(runeSlot.selectChoice as Rune);
            }
            if (runesUsedInSpell[0] == null | runesUsedInSpell[1] == null | runesUsedInSpell[2] == null)
                return NotAllRunesUsed();
            if (QuestLog.IsQuestActive("Create a Shield Spell"))
            {
                if (runesUsedInSpell[0].runeData.runeType != RuneType.StrengthenAdjacent |
                    runesUsedInSpell[1].runeData.runeType != RuneType.Shield |
                    runesUsedInSpell[2].runeData.runeType != RuneType.StrengthenAdjacent)
                    return WrongRuneArrangement();
            }
            return true;
        }

        private bool NotAllRunesUsed()
        {
            SetupDialogue(tutorialLineDatabase.didntUseAllRunes);
            return false;
        }

        private bool WrongRuneArrangement()
        {
            SetupDialogue(tutorialLineDatabase.wrongRuneArrangement);
            return false;
        }

        public void OnSpellCreated()
        {
            if (QuestLog.IsQuestActive("Create a Shield Spell"))
            {
                QuestLog.SetQuestState("Create a Shield Spell", QuestState.ReturnToNPC);
                SetupDialogue(tutorialLineDatabase.finishedFirstSpell);
            }
        }
    }
}
