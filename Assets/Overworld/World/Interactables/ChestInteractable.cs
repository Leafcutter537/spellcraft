using System.Collections;
using System.Collections.Generic;
using Assets.Combat;
using Assets.Progression;
using TMPro;
using UnityEngine;

public class ChestInteractable : Interactable
{
    public RewardData rewardData;
    [SerializeField] private RewardDistributor rewardDistributor;
    [SerializeField] private TextMeshProUGUI rewardTMP;
    public override void ShowInteractPanel()
    {
        base.ShowInteractPanel();
        string rewardText = rewardDistributor.DistributeReward(rewardData);
        rewardTMP.text = rewardText;
        Destroy(gameObject);
    }
}
