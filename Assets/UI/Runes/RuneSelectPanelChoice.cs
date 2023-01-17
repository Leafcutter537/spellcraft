
using System;
using Assets.EventSystem;
using Assets.Inventory.Runes;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class RuneSelectPanelChoice : DragPanelChoice
{
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private RuneInScrollChangedEvent runeInScrollChangedEvent;
    [SerializeField] private SpellCreatedEvent spellCreatedEvent;

    protected override void OnDisable()
    {
        if (spellCreatedEvent)
        {
            spellCreatedEvent.RemoveListener(OnSpellCreatedEvent);
        }
        base.OnDisable();
    }
    protected override void OnEnable()
    {
        if (spellCreatedEvent)
        {
            spellCreatedEvent.AddListener(OnSpellCreatedEvent);
        }
        base.OnEnable();
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        isHoveringThis = true;
        if (selectChoice != null)
        {
            enterTooltipEvent.Raise(this, new RuneEventParameters(selectChoice as Rune));
        }
    }
    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (selectChoice != null)
        {
            ClearIcon();
            startDragEvent.Raise(this, new RuneEventParameters(selectChoice as Rune));
        }
    }
    protected override void OnEndDragEvent(object sender, EventParameters args)
    {
        if (isHoveringThis)
        {
            if (!ReferenceEquals(sender, this))
            {
                if (sender is RuneSelectPanelChoice senderPanel)
                    senderPanel.TransferRune(this);
            }
        }
    }
    private void OnSpellCreatedEvent(object sender, EventParameters args)
    {
        SetChoice(null);
        runeInScrollChangedEvent.Raise(this, null);
    }
    public void TransferRune(RuneSelectPanelChoice recipient)
    {
        SelectChoice recipientSelectChoice = recipient.selectChoice;
        if (recipient.IsInScroll())
        {
            recipient.SetChoice(selectChoice);
            if (!IsInScroll())
            {
                inventoryController.HoldRune(selectChoice as Rune);
            }
        }
        if (IsInScroll())
        {
            if (!recipient.IsInScroll())
            {
                inventoryController.UnholdRune(selectChoice as Rune);
            }
            SetChoice(recipientSelectChoice);
        }
        runeInScrollChangedEvent.Raise(this, null);
    }
    public bool IsInScroll()
    {
        return selectPanel == null;
    }
}
