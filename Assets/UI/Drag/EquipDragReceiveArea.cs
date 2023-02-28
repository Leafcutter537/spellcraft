using Assets.Equipment;
using Assets.EventSystem;
using UnityEngine;

public class EquipDragReceiveArea : DragReceiveArea
{
    [SerializeField] private InventoryController inventoryController;
    protected override void ReceiveDraggedObject(object sender, EventParameters args)
    {
        if (sender is EquipmentSelectPanelChoice equipmentSlot)
        {
            inventoryController.EquipEquipment(equipmentSlot.selectChoice as EquipmentPiece);
        }
    }
}
