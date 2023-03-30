using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Combat
{
    [Serializable]
    public class EnemyShieldPattern : EnemySpellPattern
    {
        public ShieldPatternType shieldPatternType;
        public List<EnemyShieldData> shieldData;

        public enum ShieldPatternType
        {
            BlockFirstColumn,
            BlockSecondColumn,
            BlockBothColumns,
            ProjectileIndependent
        }

    }
}
