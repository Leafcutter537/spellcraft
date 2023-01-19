using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInteractable : Interactable
{
    [SerializeField] private EnemyInteractableActiveEvent enemyInteractableActiveEvent;
    public EnemyStats enemyStats;
    public override void ShowInteractPanel()
    {
        enemyInteractableActiveEvent.Raise(this, null);
    }
}
