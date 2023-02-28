
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
    public SpellPreview spellPreview;

    protected override void OnDisable()
    {
        if (spellCreatedEvent)
        {
            spellCreatedEvent.RemoveListener(OnSpellCreated);
        }
        base.OnDisable();
    }
    protected override void OnEnable()
    {
        if (spellCreatedEvent)
        {
            spellCreatedEvent.AddListener(OnSpellCreated);
        }
        base.OnEnable();
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        isHoveringThis = true;
        if (selectChoice != null)
        {
            enterTooltipEvent.Raise(this, new RuneEventParameters(selectChoice as Rune));
        }
    }

    private void OnSpellCreated(object sender, EventParameters args)
    {
        SetChoice(null);
        runeInScrollChangedEvent.Raise(this, null);
    }
    protected override void TransferItem(object sender, EventParameters args)
    {
        if (sender is RuneSelectPanelChoice senderPanel)
            senderPanel.TransferRune(this);
    }
    public void TransferRune(RuneSelectPanelChoice recipient)
    {
        if (selectChoice == null)
            return;
        SelectChoice recipientSelectChoice = recipient.selectChoice;
        if (recipient.IsInScroll())
        {
            if (!spellPreview.IsRuneSwapValid(this, recipient))
                return;
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

    private void OnDestroy()
    {
        if (selectChoice != null)
        {
            inventoryController.UnholdRune(selectChoice as Rune);
        }
    }

}
