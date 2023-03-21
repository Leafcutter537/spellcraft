using System;
using Assets.Combat;
using System.Collections.Generic;

namespace Assets.Dungeons
{
    [Serializable]
    public class DungeonLevel
    {
        public List<EnemyStats> possibleEnemies;
        public DungeonRewards rewards;
    }
}