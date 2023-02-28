using Assets.EventSystem;
using Assets.Equipment;
using UnityEngine;
using TMPro;

public class EquipmentInfoDisplay : InfoDisplay
{
    [Header("Text References")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [Header("Event References")]
    [SerializeField] private EnterTooltipEvent enterTooltipEvent;
    private void OnEnable()
    {
        enterTooltipEvent.AddListener(OnEnterTooltip);
    }
    private void OnDisable()
    {
        enterTooltipEvent.RemoveListener(OnEnterTooltip);
    }
    public override void ClearInfo()
    {
        titleText.text = "";
        descriptionText.text = "";
    }

    public override void DisplayInfo(SelectChoice selectChoice)
    {
        EquipmentPiece equipment = selectChoice as EquipmentPiece;
        titleText.text = equipment.GetTitle();
        levelText.text = "Equipment Level: " + equipment.ownedEquipmentData.currentLevel;
        descriptionText.text = equipment.GetDescription();
    }

    private void OnEnterTooltip(object sender, EventParameters args)
    {
        SelectPanelChoice selectPanelChoice = sender as SelectPanelChoice;
        if (selectPanelChoice == null)
            return;
        if (selectPanelChoice.selectChoice is EquipmentPiece equipment)
        {
            DisplayInfo(equipment);
        }
    }
}
