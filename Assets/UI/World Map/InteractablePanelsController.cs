using System.Collections;
using System.Collections.Generic;
using Assets.EventSystem;
using TMPro;
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
    [Header("Warning")]
    [SerializeField] private WarningInteractableActiveEvent warningInteractableActiveEvent;
    [SerializeField] private GameObject warningPanel;
    [SerializeField] private TextMeshProUGUI warningText;
    [Header("Leave Cell Event")]
    [SerializeField] private LeaveCellEvent leaveCellEvent;

    private void OnEnable()
    {
        enemyInteractableActiveEvent.AddListener(OnEnemyInteractableActive);
        leaveCellEvent.AddListener(OnLeaveCell);
        spellForgeInteractableEvent.AddListener(OnSpellForgeInteractable);
        shopInteractableEvent.AddListener(OnShopInteractable);
        scrapperInteractableEvent.AddListener(OnScrapperInteractable);
        warningInteractableActiveEvent.AddListener(OnWarningInteractable);
    }
    private void OnDisable()
    {
        enemyInteractableActiveEvent.RemoveListener(OnEnemyInteractableActive);
        leaveCellEvent.RemoveListener(OnLeaveCell);
        spellForgeInteractableEvent.RemoveListener(OnSpellForgeInteractable);
        shopInteractableEvent.RemoveListener(OnShopInteractable);
        scrapperInteractableEvent.RemoveListener(OnScrapperInteractable);
        warningInteractableActiveEvent.AddListener(OnWarningInteractable);
    }
    public void HideAll()
    {
        if (enemyInteractablePanel != null)
            enemyInteractablePanel.gameObject.SetActive(false);
        if (enemyDetailsPanel != null)
            enemyDetailsPanel.gameObject.SetActive(false);
        if (spellForgePanel != null)
            spellForgePanel.SetActive(false);
        if (openShopPanel != null)
            openShopPanel.SetActive(false);
        if (openScrapperPanel != null)
            openScrapperPanel.SetActive(false);
        if (warningPanel != null)
            warningPanel.SetActive(false);
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
    private void OnWarningInteractable(object sender, EventParameters args)
    {
        WarningInteractableEventParameters warningArgs = args as WarningInteractableEventParameters;
        warningPanel.SetActive(true);
        warningText.text = warningArgs.message;
    }
    private void OnLeaveCell(object sender, EventParameters args)
    {
        HideAll();
    }
}
