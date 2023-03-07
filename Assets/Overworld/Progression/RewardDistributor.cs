using System.Collections;
using System.Collections.Generic;
using Assets.Inventory.Scrolls;
using Assets.Store;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Assets.Progression
{
    [CreateAssetMenu(fileName = nameof(RewardDistributor), menuName = "ScriptableObjects/Progression/RewardDistributor")]
    public class RewardDistributor : ScriptableObject
    {
        [SerializeField] private InventoryController inventoryController;
        [SerializeField] private StoreStock storeStock;
        [SerializeField] private ScrollStock scrollStock;

        public string DistributeReward(RewardData rewardData)
        {
            string returnString = inventoryController.AddRewards(rewardData);
            foreach (ScrollData scrollData in rewardData.scrollUnlocks)
                scrollStock.AddScroll(scrollData);
            if (rewardData.quest.Length > 1)
                QuestLog.SetQuestState(rewardData.quest, QuestState.ReturnToNPC);
            return returnString;
        }
    }
}