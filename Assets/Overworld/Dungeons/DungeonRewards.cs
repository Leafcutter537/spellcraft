using System;
using Assets.Combat;
using System.Collections.Generic;
using Assets.Progression;

namespace Assets.Dungeons
{
    [Serializable]
    public class DungeonRewards
    {
        public List<float> probabilityOfReward;
        public List<RewardData> rewards;
    }
}