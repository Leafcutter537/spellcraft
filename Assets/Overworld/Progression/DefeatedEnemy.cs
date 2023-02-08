using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DefeatedEnemy
{
    public int enemyID;
    public bool isDefeated;

    public DefeatedEnemy(int enemyID, bool isDefeated)
    {
        this.enemyID = enemyID;
        this.isDefeated = isDefeated;
    }
}
