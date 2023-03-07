using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace Assets.Tutorial
{
    public class TutorialPanel : MonoBehaviour
    {
        [SerializeField] private TutorialController tutorialController;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private TextMeshProUGUI buttonText;
        private float timeSinceLineShown;
        private string lineText;

        private void Update()
        {
            if (lineText.Length > 0)
            {
                timeSinceLineShown += Time.deltaTime;
                int numCharactersShown = (int)(timeSinceLineShown * tutorialController.textSpeed);
                string visibleText = "";
                string invisibleText = "";
                if (numCharactersShown < lineText.Length)
                {
                    visibleText = lineText.Substring(0, numCharactersShown);
                    invisibleText = lineText.Substring(numCharactersShown, lineText.Length - numCharactersShown - 1);
                }
                else
                {
                    visibleText = lineText;
                }
                dialogueText.text = visibleText + "<color=#00000000>" + invisibleText + "</color>";
            }
        }

        public void ShowLine(string lineText, bool isLast)
        {
            timeSinceLineShown = 0;
            this.lineText = lineText;
            buttonText.text = isLast ?  "Close" : "Next";
        }
    }
}