using Assets.Combat;
using Assets.EventSystem;
using Assets.Inventory.Runes;
using Assets.Inventory.Spells;
using TMPro;
using UnityEngine;

public class SpellInfoDisplay : InfoDisplay
{
    [Header("Text References")]
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI manaCostText;
    [Header("Event References")]
    [SerializeField] private EnterTooltipEvent enterTooltipEvent;
    [Header("Stat References")]
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private CharacterInstance characterInstance;

    private void OnEnable()
    {
        if (enterTooltipEvent != null)
            enterTooltipEvent.AddListener(OnEnterTooltip);
    }
    private void OnDisable()
    {
        if (enterTooltipEvent != null)
            enterTooltipEvent.RemoveListener(OnEnterTooltip);
    }
    public override void DisplayInfo(SelectChoice selectChoice)
    {
        DisplaySpellInfo(selectChoice as Spell);
    }
    public void DisplaySpellInfo(Spell spell)
    {
        if (characterInstance != null)
            descriptionText.text = spell.GetDescription(characterInstance.GetStatBundle());
        else if (playerStats != null)
            descriptionText.text = spell.GetDescription(playerStats.GetStatBundle());
        else
            descriptionText.text = spell.GetDescription();

        manaCostText.text = "Mana Cost: " + spell.manaCost;
    }
    public override void ClearInfo()
    {
        descriptionText.text = "";
        manaCostText.text = "";
    }
    private void OnEnterTooltip(object sender, EventParameters args)
    {
        SelectPanelChoice selectPanelChoice = sender as SelectPanelChoice;
        if (selectPanelChoice == null)
            return;
        if (selectPanelChoice.selectChoice is Spell spell)
        {
            DisplaySpellInfo(spell);
        }
    }
}
