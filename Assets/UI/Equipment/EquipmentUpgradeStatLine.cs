using TMPro;
using UnityEngine;

public class EquipmentUpgradeStatLine : MonoBehaviour
{
    public TextMeshProUGUI currentValueText;
    public TextMeshProUGUI nextValueText;
    public TextMeshProUGUI statNameText;

    public void SetStatLine(int currentValue, int nextValue, string statName)
    {
        currentValueText.text = currentValue.ToString();
        nextValueText.text = nextValue.ToString();
        statNameText.text = statName;
    }
}
