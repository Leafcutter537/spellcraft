using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(ProgressTracker), menuName = "ScriptableObjects/Progression/ProgressTracker")]
public class ProgressTracker : ScriptableObject
{
    public Dictionary<int, bool> defeatedEnemies;
    public Vector2Int playerPosition;
    public bool loadDevProgress;

    public void AddDefeatedEnemy(int enemyID)
    {
        defeatedEnemies.TryAdd(enemyID, true);
    }
}
