using System.Collections;
using System.Collections.Generic;
using Assets.Combat;
using Assets.Equipment;
using Assets.Inventory.Runes;
using Assets.Progression;
using TMPro;
using UnityEngine;

public class EndCombatPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mainText;
    [SerializeField] private TextMeshProUGUI detailText;

    public void ShowVictory(string rewardText)
    {
        mainText.text = "Victory!";
        detailText.text = rewardText;
    }

    public void ShowDefeat()
    {
        mainText.text = "Defeat!";
        detailText.text = "You have been defeated!";
    }
}
