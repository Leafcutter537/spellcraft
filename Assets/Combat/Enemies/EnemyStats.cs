using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Combat
{
    [Serializable]
    public class EnemyStats
    {
        public string enemyName;
        public int enemyID;
        public int maxHP;
        public int resilience;
        public List<EnemyProjectilePattern> projectilePatterns;
        public List<EnemyShieldPattern> shieldPatterns;
        public List<EnemyHealPattern> healPatterns;
        public List<EnemyBuffPattern> buffPatterns;

        public StatBundle GetStatBundle()
        {
            return new StatBundle(maxHP, 0, resilience, 0, 0, 0);
        }
    }
}
