using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(ProgressTracker), menuName = "ScriptableObjects/Progression/ProgressTracker")]
public class ProgressTracker : ScriptableObject
{
    public Dictionary<int, bool> defeatedEnemies;
    public Vector2Int playerPosition;
    public bool justDefeatedEnemy;
    public int justDefeatedEnemyIndex;
    public bool loadDevProgress;

    public void AddDefeatedEnemy(int enemyID)
    {
        if (enemyID != -1)
        {
            defeatedEnemies.TryAdd(enemyID, true);
        }
        justDefeatedEnemy = true;
        justDefeatedEnemyIndex = enemyID;
    }
}
