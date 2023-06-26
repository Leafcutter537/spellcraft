using System.Collections;
using System.Collections.Generic;
using Assets.Tutorial;
using UnityEngine;

namespace Assets.Tutorial
{
    [CreateAssetMenu(fileName = nameof(TutorialLineDatabase), menuName = "ScriptableObjects/Progression/TutorialLineDatabase")]
    public class TutorialLineDatabase : ScriptableObject
    {
        public List<TutorialLine> firstCombatStartLines;
        public List<TutorialLine> firstCombatSelectSpell;
        public List<TutorialLine> firstCombatSelectPath;
        public List<TutorialLine> firstCombatCastThirdSpell;
        public List<TutorialLine> firstCombatStartSecondPlayerTurn;
        public List<TutorialLine> secondCombatStartLines;
        public List<TutorialLine> createFirstSpell;
        public List<TutorialLine> didntUseAllRunes;
        public List<TutorialLine> wrongRuneArrangement;
        public List<TutorialLine> finishedFirstSpell;
        public List<TutorialLine> thirdCombatStartLines;
        public List<TutorialLine> thirdCombatStartSecondPlayerTurn;
    }
}