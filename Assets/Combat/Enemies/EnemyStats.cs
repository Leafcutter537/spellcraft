using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Combat.Enemy;
using UnityEngine;

[Serializable]
public class EnemyStats
{
    public string enemyName;
    public int enemyID;
    public int maxHP;
    public int maxMP;
    public List<EnemySpellData> spells;
}
