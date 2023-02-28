using System.Collections.Generic;
using UnityEngine;

namespace Assets.Progression
{
    [CreateAssetMenu(fileName = nameof(RewardDatabase), menuName = "ScriptableObjects/Progression/RewardDatabase")]
    public class RewardDatabase : ScriptableObject
    {
        public Dictionary<int, RewardData> rewards;
        public int[] enemyIDs;
        public RewardData[] rewardData;

        public RewardData GetReward(int enemyID)
        {
            if (rewards == null)
            {
                InitalizeDictionary();
            }
            if (rewards.TryGetValue(enemyID, out RewardData returnValue))
                return returnValue;
            else
                return null;
        }

        private void InitalizeDictionary()
        {
            rewards = new Dictionary<int, RewardData>();
            for (int i = 0; i < enemyIDs.Length; i++)
            {
                rewards.Add(enemyIDs[i], rewardData[i]);
            }
        }
    }
}
