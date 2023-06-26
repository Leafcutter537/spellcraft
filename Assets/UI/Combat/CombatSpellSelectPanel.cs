using System.Collections;
using System.Collections.Generic;
using Assets.Combat;
using Assets.EventSystem;
using Assets.Inventory.Spells;
using TMPro;
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
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI instructionText;
    [Header("Event References")]
    [SerializeField] private StartSpellPreviewEvent startSpellPreviewEvent;
    [SerializeField] private EndSpellPreviewEvent endSpellPreviewEvent;
    [SerializeField] private TooltipWarningEvent tooltipWarningEvent;
    [SerializeField] private StartAdjacentVectorPreviewEvent startAdjacentVectorPreviewEvent;
    [SerializeField] private CastUntargetedSpellEvent castUntargetedSpellEvent;

    private void OnEnable()
    {
        startAdjacentVectorPreviewEvent.AddListener(OnStartAdjacentVectorPreviewEvent);
    }
    private void OnDisable()
    {
        startAdjacentVectorPreviewEvent.RemoveListener(OnStartAdjacentVectorPreviewEvent);
    }
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
        else if (!playerInstance.HasSufficientMana(GetSelected() as Spell))
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
            Spell spellBeingCast = GetSelected() as Spell;
            if (spellBeingCast.targetType == TargetType.Projectile | spellBeingCast.targetType == TargetType.Shield)
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
            else
            {
                castUntargetedSpellEvent.Raise(this, null);
                Deselect();
            }
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
        instructionText.text = "Choose a square to cast spell";
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, defaultHeight);
        rectTransform.position = new Vector2(rectTransform.position.x, rectTransform.position.y - (defaultHeight - shrunkHeight) / 2);
    }

    private void OnStartAdjacentVectorPreviewEvent(object sender, EventParameters args)
    {
        AdjacentPreviewEventParameters adjacentArgs = args as AdjacentPreviewEventParameters;
        instructionText.text = "Choose a direction for adjacent " + adjacentArgs.entityName;
    }
}
