using Assets.EventSystem;
using Assets.Inventory.Runes;
using TMPro;
using UnityEngine;

public class SpellInfoDisplay : InfoDisplay
{
    [Header("Text References")]
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI manaCostText;

    public override void DisplayInfo(SelectChoice selectChoice)
    {
        DisplaySpellInfo(selectChoice as Spell);
    }
    public void DisplaySpellInfo(Spell spell)
    {
        descriptionText.text = spell.GetDescription();
        manaCostText.text = "Mana Cost: " + spell.manaCost;
    }
    public override void ClearInfo()
    {
        descriptionText.text = "";
        manaCostText.text = "";
    }
}
