using System.Collections;
using System.Collections.Generic;
using Assets.EventSystem;
using UnityEngine;

public class InteractablePanelsController : MonoBehaviour
{
    [SerializeField] private EnemyInteractablePanel enemyInteractablePanel;
    [SerializeField] private EnemyInteractableActiveEvent enemyInteractableActiveEvent;
    [SerializeField] private LeaveCellEvent leaveCellEvent;

    private void OnEnable()
    {
        enemyInteractableActiveEvent.AddListener(OnEnemyInteractableActive);
        leaveCellEvent.AddListener(OnLeaveCell);
    }
    private void OnDisable()
    {
        enemyInteractableActiveEvent.RemoveListener(OnEnemyInteractableActive);
        leaveCellEvent.RemoveListener(OnLeaveCell);
    }
    private void HideAll()
    {
        enemyInteractablePanel.gameObject.SetActive(false);
    }
    private void OnEnemyInteractableActive(object sender, EventParameters args)
    {
        enemyInteractablePanel.gameObject.SetActive(true);
        EnemyInteractable enemyInteractable = sender as EnemyInteractable;
        enemyInteractablePanel.SetEnemyStatInfo(enemyInteractable.enemyStats);
    }
    private void OnLeaveCell(object sender, EventParameters args)
    {
        HideAll();
    }
}
