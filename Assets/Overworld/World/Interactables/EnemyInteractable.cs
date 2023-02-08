using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInteractable : Interactable
{
    [SerializeField] private EnemyInteractableActiveEvent enemyInteractableActiveEvent;
    public EnemyStats enemyStats;
    [SerializeField] private int worldIndex;
    [SerializeField] private ProgressTracker progressTracker;

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
        enemyInteractableActiveEvent.Raise(this, null);
    }
}
