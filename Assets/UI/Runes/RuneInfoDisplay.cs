using Assets.EventSystem;
using Assets.Inventory.Runes;
using TMPro;
using UnityEngine;

public class RuneInfoDisplay : MonoBehaviour
{
    [Header("Text References")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI qualityText;
    [SerializeField] private TextMeshProUGUI manaCostText;
    [Header("Event References")]
    [SerializeField] private EnterTooltipEvent enterTooltipEvent;

    public void DisplayRuneInfo(Rune rune)
    {
        titleText.text = rune.GetTitle();
        descriptionText.text = rune.GetDescription();
        qualityText.text = "Quality: " + rune.runeData.quality;
        if (Mathf.Abs(rune.manaCost) > 0.001f)
            manaCostText.text = "Mana Cost: " + Rune.GetStringOfRuneValue(rune.manaCost);
        else
            manaCostText.text = "";
    }
    private void OnEnable()
    {
        enterTooltipEvent.AddListener(OnEnterTooltip);
    }
    private void OnDisable()
    {
        enterTooltipEvent.RemoveListener(OnEnterTooltip);
    }
    private void OnEnterTooltip(object sender, EventParameters args)
    {
        SelectPanelChoice selectPanelChoice = sender as SelectPanelChoice;
        if (selectPanelChoice == null)
            return;
        if (selectPanelChoice.selectChoice is Rune rune)
        {
            DisplayRuneInfo(rune);
        }
    }
}
