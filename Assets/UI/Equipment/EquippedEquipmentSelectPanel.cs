using Assets.Equipment;
using Assets.EventSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedEquipmentSelectPanel : SelectPanel
{
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private EquipmentChangeEvent equipmentChangeEvent;
    [SerializeField] private PlayerStatTotal playerStatTotal;

    private void OnEnable()
    {
        equipmentChangeEvent.AddListener(OnEquipmentChange);
    }
    private void OnDisable()
    {
        equipmentChangeEvent.RemoveListener(OnEquipmentChange);
    }
    protected override void GetInventory()
    {
        itemList = new List<SelectChoice>();
        for (int i = 0; i < 5; i++) itemList.Add(null);
        List<EquipmentPiece> equipment = inventoryController.GetEquippedEquipment();
        foreach (EquipmentPiece equ in equipment)
        {
            itemList[(int)equ.ownedEquipmentData.equipmentSlot] = equ;
        }
        playerStatTotal.UpdatePlayerStats();
    }
    private void OnEquipmentChange(object sender, EventParameters args)
    {
        RefreshInventory();
    }
}
