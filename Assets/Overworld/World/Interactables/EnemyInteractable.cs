using System.Collections;
using System.Collections.Generic;
using Assets.Combat;
using UnityEngine;

public class EnemyInteractable : Interactable
{
    public EnemyStats enemyStats;
    [SerializeField] private ProgressTracker progressTracker;
    [SerializeField] private EnemyInteractablePanel enemyInteractablePanel;

    protected override void Start()
    {
        if (progressTracker.defeatedEnemies.TryGetValue(enemyStats.enemyID, out bool isDefeated))
        {
            if (isDefeated)
            {
                Destroy(gameObject);
                return;
            }
        }
        base.Start();
    }
    public override void ShowInteractPanel()
    {
        enemyInteractablePanel.gameObject.SetActive(true);
        enemyInteractablePanel.SetEnemyStatInfo(enemyStats);
    }
}
