using System.Collections;
using System.Collections.Generic;
using Assets.EventSystem;
using UnityEngine;

public class RuneSelectPanel : SelectPanel
{
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private HoldRuneEvent holdRuneEvent;

    protected override void Awake()
    {
        inventoryController.ClearHeldRunes();
        base.Awake();
    }
    private void OnEnable()
    {
        holdRuneEvent.AddListener(OnHoldRuneEvent);
    }
    private void OnDisable()
    {
        holdRuneEvent.RemoveListener(OnHoldRuneEvent);
    }
    protected override void GetInventory()
    {
        itemList = inventoryController.GetRuneList();
    }
    
    private void OnHoldRuneEvent(object sender, EventParameters args)
    {
        RefreshInventory();
    }
}
