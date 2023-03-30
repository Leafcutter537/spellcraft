using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Combat
{
    [Serializable]
    public class EnemyProjectilePattern : EnemySpellPattern
    {
        public ProjectilePatternType projectilePatternType;
        public List<EnemyProjectileData> projectileData;

        public enum ProjectilePatternType 
        {
            RandomUnoccupied,
            SetPosition
        }

    }
}
