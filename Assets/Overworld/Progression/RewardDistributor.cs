using System.Collections;
using System.Collections.Generic;
using Assets.Combat;
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
        [SerializeField] private RewardDatabase rewardDatabase;
        [SerializeField] private ProgressTracker progressTracker;

        public string DistributeReward(int enemyIndex)
        {
            progressTracker.AddDefeatedEnemy(enemyIndex);
            RewardData rewardData = rewardDatabase.GetReward(enemyIndex);
            return DistributeReward(rewardData);

        }

        public string DistributeReward(RewardData rewardData)
        {
            if (rewardData == null)
                return null;
            string returnString = inventoryController.AddRewards(rewardData);
            returnString = scrollStock.AddScrolls(rewardData.scrollUnlocks, returnString);
            returnString = storeStock.AddStoreStock(rewardData.runeStockUnlocks, returnString);
            if (rewardData.quest.Length > 1)
                QuestLog.SetQuestState(rewardData.quest, QuestState.ReturnToNPC);
            return returnString;
        }
    }
}