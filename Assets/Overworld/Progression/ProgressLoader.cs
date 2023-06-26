using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressLoader : MonoBehaviour
{
    [SerializeField] private ProgressTracker progressTracker;
    [SerializeField] private DevProgress devProgress;
    private static bool hasLoadedProgress;

    private void Awake()
    {
        if (progressTracker.loadDevProgress & !hasLoadedProgress & Application.isEditor)
        {
            LoadDefeatedEnemies(devProgress.firstDefeatEnemies);
            progressTracker.playerPosition = devProgress.playerPosition;
            hasLoadedProgress = true;
        }
        else if (!hasLoadedProgress)
        {
            if (SaveManager.HasSaveData())
            {
                // Load Save
            }
            else
            {
                progressTracker.defeatedEnemies = new Dictionary<int, bool>();
                progressTracker.playerPosition = Vector2Int.zero;
                hasLoadedProgress = true;
            }
        }
    }

    private void LoadDefeatedEnemies(List<DefeatedEnemy> defeatedEnemies)
    {
        progressTracker.defeatedEnemies = new Dictionary<int, bool>();
        foreach (DefeatedEnemy defeatedEnemy in defeatedEnemies)
        {
            progressTracker.defeatedEnemies.Add(defeatedEnemy.enemyID, defeatedEnemy.isDefeated);
        }
    }
}
