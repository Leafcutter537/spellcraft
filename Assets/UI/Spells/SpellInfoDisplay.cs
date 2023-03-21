using System.Collections.Generic;
using Assets.Combat;
using Assets.EventSystem;
using Assets.Inventory.Runes;
using Assets.Inventory.Spells;
using TMPro;
using UnityEngine;

public class SpellInfoDisplay : InfoDisplay
{
    [Header("Prefab References")]
    [SerializeField] private GameObject tooltipTextPrefab;
    [Header("UI References")]
    [SerializeField] private Transform textboxParent;
    // Instances of created objects
    private List<GameObject> tooltipTextObjects;

    private void Awake()
    {
        tooltipTextObjects = new List<GameObject>();
    }

    private void DisplayText(string text, float size, Color color)
    {
        GameObject tooltipText = Instantiate(tooltipTextPrefab);
        tooltipText.transform.SetParent(textboxParent, false);
        tooltipTextObjects.Add(tooltipText);
        TextMeshProUGUI tmp = tooltipText.GetComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = size;
        tmp.color = color;
    }
    public override void DisplayInfo(SelectChoice selectChoice)
    {
        DisplaySpellInfo(selectChoice as Spell);
    }
    public void DisplaySpellInfo(Spell spell)
    {
        ClearInfo();
        DisplayText("Mana Cost: " + spell.manaCost.ToString(), 14, new Color(0, 30f / 255, 116f / 255));
        if (spell.cooldown > 0)
            DisplayText("Cooldown: " + spell.cooldown.ToString(), 14, new Color(30f / 255, 30f / 255, 0));
        if (spell.chargeTime > 0)
            DisplayText("Charge Time: " + spell.chargeTime.ToString(), 14, new Color(30f / 255, 30f / 255, 0));
        string descriptionText = spell.GetDescription();
        DisplayText(descriptionText, 14, Color.black);
    }
    public override void ClearInfo()
    {
        foreach (GameObject obj in tooltipTextObjects)
            Destroy(obj);
        tooltipTextObjects = new List<GameObject>();
    }

}
