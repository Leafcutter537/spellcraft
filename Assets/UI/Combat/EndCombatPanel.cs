using System.Collections;
using System.Collections.Generic;
using Assets.Combat;
using Assets.Inventory.Runes;
using Assets.Progression;
using TMPro;
using UnityEngine;

public class EndCombatPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mainText;
    [SerializeField] private TextMeshProUGUI detailText;
    [SerializeField] private RewardDatabase rewardDatabase;
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private EnemyInstance enemyInstance;
    [SerializeField] private RuneGenerator runeGenerator;

    public void ShowVictory()
    {
        RewardData rewardData = rewardDatabase.GetReward(enemyInstance.enemyID);
        inventoryController.AddRewards(rewardData);
        mainText.text = "Victory!";
        detailText.text = "";
        foreach (RuneData runeData in rewardData.runeRewards)
        {
            Rune rune = runeGenerator.CreateRune(runeData);
            detailText.text += "Received a " + rune.GetTitle() + " rune of quality " + runeData.quality + "!\n\n";
        }
    }

    public void ShowDefeat()
    {
        mainText.text = "Defeat!";
        detailText.text = "You have been defeated!";
    }
}
