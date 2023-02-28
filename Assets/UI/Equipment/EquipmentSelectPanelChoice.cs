using Assets.Equipment;
using Assets.EventSystem;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSelectPanelChoice : DragPanelChoice
{
    public bool isEquippedSlot;
    [SerializeField] private InventoryController inventoryController;
    protected override void Awake()
    {
        SetDefaultSprite(icon.sprite);
        base.Awake();
    }
    protected override void TransferItem(object sender, EventParameters args)
    {
        if (sender is EquipmentSelectPanelChoice receivingSlot)
        {
            if (receivingSlot.isEquippedSlot & !isEquippedSlot)
            {
                inventoryController.EquipEquipment(selectChoice as EquipmentPiece);
            }
        }
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        isHoveringThis = true;
        if (selectChoice != null)
        {
            enterTooltipEvent.Raise(this, null);
        }
    }
}