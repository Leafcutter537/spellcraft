using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Combat.Enemy;
using UnityEngine;

namespace Assets.Combat
{
    [Serializable]
    public class EnemyStats
    {
        public string enemyName;
        public int enemyID;
        public int maxHP;
        public int maxMP;
        public int resilience;
        public List<EnemySpellData> spells;

        public StatBundle GetStatBundle()
        {
            return new StatBundle(maxHP, maxMP, resilience, 0, 0, 0);
        }
    }
}
