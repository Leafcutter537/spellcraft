using System.Collections;
using System.Collections.Generic;
using Assets.EventSystem;
using UnityEditor;
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
    [Header("Shop")]
    [SerializeField] private ShopInteractableEvent shopInteractableEvent;
    [SerializeField] private GameObject openShopPanel;
    [Header("Scrapper")]
    [SerializeField] private ScrapperInteractableEvent scrapperInteractableEvent;
    [SerializeField] private GameObject openScrapperPanel;
    [Header("Leave Cell Event")]
    [SerializeField] private LeaveCellEvent leaveCellEvent;

    private void OnEnable()
    {
        enemyInteractableActiveEvent.AddListener(OnEnemyInteractableActive);
        leaveCellEvent.AddListener(OnLeaveCell);
        spellForgeInteractableEvent.AddListener(OnSpellForgeInteractable);
        shopInteractableEvent.AddListener(OnShopInteractable);
        scrapperInteractableEvent.AddListener(OnScrapperInteractable);
    }
    private void OnDisable()
    {
        enemyInteractableActiveEvent.RemoveListener(OnEnemyInteractableActive);
        leaveCellEvent.RemoveListener(OnLeaveCell);
        spellForgeInteractableEvent.RemoveListener(OnSpellForgeInteractable);
        shopInteractableEvent.RemoveListener(OnShopInteractable);
        scrapperInteractableEvent.RemoveListener(OnScrapperInteractable);
    }
    private void HideAll()
    {
        enemyInteractablePanel.gameObject.SetActive(false);
        enemyDetailsPanel.gameObject.SetActive(false);
        spellForgePanel.SetActive(false);
        openShopPanel.SetActive(false);
        openScrapperPanel.SetActive(false);
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
    private void OnShopInteractable(object sender, EventParameters args)
    {
        openShopPanel.SetActive(true);
    }
    private void OnScrapperInteractable(object sender, EventParameters args)
    {
        openScrapperPanel.SetActive(true);
    }
    private void OnLeaveCell(object sender, EventParameters args)
    {
        HideAll();
    }
}
