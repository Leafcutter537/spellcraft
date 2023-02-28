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
    [SerializeField] private RewardDatabase rewardDatabase;
    [SerializeField] private RewardDistributor distributor;
    [SerializeField] private EnemyInstance enemyInstance;

    public void ShowVictory()
    {
        RewardData rewardData = rewardDatabase.GetReward(enemyInstance.enemyID);
        string rewardText = distributor.DistributeReward(rewardData);
        mainText.text = "Victory!";
        detailText.text = rewardText;
    }

    public void ShowDefeat()
    {
        mainText.text = "Defeat!";
        detailText.text = "You have been defeated!";
    }
}
