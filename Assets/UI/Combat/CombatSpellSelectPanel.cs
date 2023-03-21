using System.Collections;
using System.Collections.Generic;
using Assets.Combat;
using Assets.Inventory.Spells;
using UnityEngine;

public class CombatSpellSelectPanel : SelectPanel
{
    [Header("Serialized Object References")]
    [SerializeField] private InventoryController inventoryController;
    [Header("Height Adjustment")]
    [SerializeField] private float defaultHeight;
    [SerializeField] private float shrunkHeight;
    [SerializeField] private RectTransform rectTransform;
    [Header("Scene References")]
    [SerializeField] private PlayerInstance playerInstance;
    [SerializeField] private TurnController turnController;
    [Header("Event References")]
    [SerializeField] private StartSpellPreviewEvent startSpellPreviewEvent;
    [SerializeField] private EndSpellPreviewEvent endSpellPreviewEvent;
    [SerializeField] private TooltipWarningEvent tooltipWarningEvent;
    protected override void GetInventory()
    {
        itemList = inventoryController.GetEquippedSpellList();
        List<Spell> spellList = new List<Spell>();
        foreach (SelectChoice selectChoice in itemList)
        {
            spellList.Add(selectChoice as Spell);
        }
        playerInstance.SetStartingCooldowns(spellList);
    }

    protected override void SelectUpdate()
    {
        if (!(turnController.isPlayerTurn & turnController.turnStage == TurnController.TurnStage.CharacterActing))
        {
            Deselect();
        }
        if (!playerInstance.HasSufficientMana(GetSelected() as Spell))
        {
            tooltipWarningEvent.Raise(this, new TooltipWarningEventParameters("Insufficient Mana!"));
            Deselect();
        }
        else if (playerInstance.GetSpellCooldown(GetIndex()) > 0)
        {
            tooltipWarningEvent.Raise(this, new TooltipWarningEventParameters("That spell is on cooldown!"));
            Deselect();
        }
        else
        {
            for (int i = 0; i < selectPanelChoices.Count; i++)
            {
                if (i != selectedIndex)
                {
                    selectPanelChoices[i].gameObject.SetActive(false);
                }
            }
            startSpellPreviewEvent.Raise(this, null);
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, shrunkHeight);
            rectTransform.position = new Vector2(rectTransform.position.x, rectTransform.position.y + (defaultHeight - shrunkHeight) / 2);
        }
    }
    public void ReturnSpellList()
    {
        foreach (SelectPanelChoice selectPanelChoice in selectPanelChoices)
        {
            selectPanelChoice.gameObject.SetActive(true);
        }
        Deselect();
        endSpellPreviewEvent.Raise(this, null);
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, defaultHeight);
        rectTransform.position = new Vector2(rectTransform.position.x, rectTransform.position.y - (defaultHeight - shrunkHeight) / 2);
    }
}
