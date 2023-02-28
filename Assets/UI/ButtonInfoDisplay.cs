using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInfoDisplay : InfoDisplay
{
    [SerializeField] private List<Button> buttons;
    public override void ClearInfo()
    {
        foreach (Button button in buttons)
            button.interactable = false;
    }

    public override void DisplayInfo(SelectChoice selectChoice)
    {
        if (selectChoice != null)
        {
            foreach (Button button in buttons)
                button.interactable = true;
        }
    }
}
