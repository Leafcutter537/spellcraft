using System.Collections;
using System.Collections.Generic;
using Assets.EventSystem;
using UnityEngine;

public class InteractablePanelsController : MonoBehaviour
{
    [Header("Enemy Encounter")]
    [SerializeField] private EnemyInteractablePanel enemyInteractablePanel;
    [SerializeField] private EnemyInteractableActiveEvent enemyInteractableActiveEvent;
    [SerializeField] private EnemyDetailsPanel enemyDetailsPanel;
    [Header("Spell Forge")]
    [SerializeField] private SpellForgeInteractableEvent spellForgeInteractableEvent;
    [SerializeField] private GameObject spellForgePanel;
    [Header("Leave Cell Event")]
    [SerializeField] private LeaveCellEvent leaveCellEvent;

    private void OnEnable()
    {
        enemyInteractableActiveEvent.AddListener(OnEnemyInteractableActive);
        leaveCellEvent.AddListener(OnLeaveCell);
        spellForgeInteractableEvent.AddListener(OnSpellForgeInteractable);
    }
    private void OnDisable()
    {
        enemyInteractableActiveEvent.RemoveListener(OnEnemyInteractableActive);
        leaveCellEvent.RemoveListener(OnLeaveCell);
        spellForgeInteractableEvent.RemoveListener(OnSpellForgeInteractable);
    }
    private void HideAll()
    {
        enemyInteractablePanel.gameObject.SetActive(false);
        enemyDetailsPanel.gameObject.SetActive(false);
        spellForgePanel.SetActive(false);
    }
    private void OnEnemyInteractableActive(object sender, EventParameters args)
    {
        enemyInteractablePanel.gameObject.SetActive(true);
        EnemyInteractable enemyInteractable = sender as EnemyInteractable;
        enemyInteractablePanel.SetEnemyStatInfo(enemyInteractable.enemyStats);
    }
    private void OnSpellForgeInteractable(object sender, EventParameters args)
    {
        spellForgePanel.SetActive(true);
    }
    private void OnLeaveCell(object sender, EventParameters args)
    {
        HideAll();
    }
}
