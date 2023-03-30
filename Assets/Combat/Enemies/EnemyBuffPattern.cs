using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Combat
{
    [Serializable]
    public class EnemyBuffPattern : EnemySpellPattern
    {
        public List<EnemyBuffData> enemyBuffData;
    }
}
