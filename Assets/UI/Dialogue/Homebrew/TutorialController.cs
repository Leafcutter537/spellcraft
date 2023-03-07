using System.Collections;
using System.Collections.Generic;
using Assets.Tutorial;
using UnityEngine;

namespace Assets.Tutorial
{
    public class TutorialController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject tutorialPanelsParent;
        [SerializeField] private List<TutorialPanel> tutorialPanels;
        [SerializeField] private GameObject replayTutorialPanel;
        // Lines
        protected List<TutorialLine> activeLines;
        private int currentLineIndex;
        [Header("Text Settings")]
        public float textSpeed;
        // Sequencing
        [HideInInspector] public bool isShowingTutorial;

        protected void SetupDialogue(List<TutorialLine> lines)
        {
            activeLines = lines;
            replayTutorialPanel.SetActive(true);
            InitiateDialogue();
        }
        public void InitiateDialogue()
        {
            currentLineIndex = 0;
            tutorialPanelsParent.SetActive(true);
            isShowingTutorial = true;
            ShowLine();
        }
        private void ShowLine()
        {
            foreach (TutorialPanel tutorialPanel in tutorialPanels)
            {
                tutorialPanel.gameObject.SetActive(false);
            }
            TutorialLine thisLine = activeLines[currentLineIndex];
            tutorialPanels[thisLine.panelIndex].gameObject.SetActive(true);
            tutorialPanels[thisLine.panelIndex].ShowLine(thisLine.text, currentLineIndex == activeLines.Count - 1);
        }
        public void OnClickNext()
        {
            currentLineIndex++;
            if (currentLineIndex >= activeLines.Count)
            {
                tutorialPanelsParent.gameObject.SetActive(false);
                isShowingTutorial = false;
            }
            else
            {
                ShowLine();
            }
        }
    }
}
