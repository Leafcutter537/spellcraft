using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressLoader : MonoBehaviour
{
    [SerializeField] private ProgressTracker progressTracker;
    [SerializeField] private DevProgress devProgress;
    private static bool hasLoadedDevProgress;

    private void Awake()
    {
        if (progressTracker.loadDevProgress & !hasLoadedDevProgress & Application.isEditor)
        {
            LoadDefeatedEnemies(devProgress.firstDefeatEnemies);
            progressTracker.playerPosition = devProgress.playerPosition;
            hasLoadedDevProgress = true;
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
