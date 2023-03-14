using System.Collections;
using System.Collections.Generic;
using System.Data;
using Assets.Combat;
using Assets.Combat.SpellEffects;
using Assets.Equipment;
using Assets.EventSystem;
using Assets.Inventory.Runes;
using Assets.Inventory.Spells;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TooltipDisplay : MonoBehaviour
{
    [Header("Stat References")]
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private CharacterInstance characterInstance;
    [Header("Event References")]
    [SerializeField] private EnterTooltipEvent enterTooltipEvent;
    [SerializeField] private ExitTooltipEvent exitTooltipEvent;
    [SerializeField] private MouseEnterPathEvent mouseEnterPathEvent;
    [SerializeField] private MouseExitPathEvent mouseExitPathEvent;
    [Header("Prefabs")]
    [SerializeField] private GameObject tooltipTextPrefab;
    // Instances of created objects
    private List<GameObject> tooltipTextObjects;

    private void Awake()
    {
        tooltipTextObjects = new List<GameObject>();
    }

    private void OnEnable()
    {
        enterTooltipEvent.AddListener(OnEnterTooltip);
        exitTooltipEvent.AddListener(OnExitTooltip);
        mouseEnterPathEvent.AddListener(OnMouseEnterPath);
        mouseExitPathEvent.AddListener(OnMouseExitPath);
    }
    private void OnDisable()
    {
        enterTooltipEvent.RemoveListener(OnEnterTooltip);
        exitTooltipEvent.RemoveListener(OnExitTooltip);
        mouseEnterPathEvent.RemoveListener(OnMouseEnterPath);
        mouseExitPathEvent.RemoveListener(OnMouseExitPath);
    }
    private void OnEnterTooltip(object sender, EventParameters args)
    {
        CharacterInstance characterInstance = sender as CharacterInstance;
        if (characterInstance != null)
        {
            DisplayCharacterInstanceInfo(characterInstance);
        }
        SelectPanelChoice selectPanelChoice = sender as SelectPanelChoice;
        if (selectPanelChoice == null)
            return;
        if (selectPanelChoice.selectChoice is Rune rune)
        {
            DisplayRuneInfo(rune);
        }
        else if (selectPanelChoice.selectChoice is Spell spell)
        {
            DisplaySpellInfo(spell);
        }
        else if (selectPanelChoice.selectChoice is EquipmentPiece equipmentPiece)
        {
            DisplayEquipmentInfo(equipmentPiece);
        }
    }
    private void OnExitTooltip(object sender, EventParameters args)
    {
        foreach (GameObject obj in tooltipTextObjects)
            Destroy(obj);
        tooltipTextObjects = new List<GameObject>();
    }

    private void OnMouseEnterPath(object sender, EventParameters args)
    {
        Path path = sender as Path;
        DisplayText(path.pathName, 18, Color.black);
        if (path.playerProjectile != null)
        {
            DisplayText(path.playerProjectile.GetProjectileDescription(), 14, Color.black);
        }
        if (path.enemyProjectile != null)
        {
            DisplayText(path.enemyProjectile.GetProjectileDescription(), 14, Color.black);
        }
        if (path.playerShield != null)
        {
            DisplayText(path.playerShield.GetShieldDescription(), 14, Color.black);
        }
        if (path.enemyShield != null)
        {
            DisplayText(path.enemyShield.GetShieldDescription(), 14, Color.black);
        }
    }
    private void OnMouseExitPath(object sender, EventParameters args)
    {
        OnExitTooltip(sender, args);
    }

    private void DisplayText(string text, float size, Color color)
    {
        GameObject tooltipText = Instantiate(tooltipTextPrefab);
        tooltipText.transform.SetParent(transform, false);
        tooltipTextObjects.Add(tooltipText);
        TextMeshProUGUI tmp = tooltipText.GetComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = size;
        tmp.color = color;
    }

    private void DisplayRuneInfo(Rune rune)
    {
        DisplayText(rune.GetTitle(), 18, Color.black);
        DisplayText(rune.runeData.GetRuneCategoryString() + " Rune", 14, new Color(30f/255, 30f/255, 0));
        DisplayText("Quality: " + rune.runeData.quality.ToString(), 14, new Color(50f / 255, 0, 0));
        if (rune.manaCost > 0)
            DisplayText("Mana Cost: " + rune.manaCost.ToString(), 14, new Color(0, 30f/255, 116f/255));
        switch (rune.runeData.requiredPrimary)
        {
            case RequiredPrimary.Projectile:
                DisplayText("Requires a Projectile Rune", 14, new Color(50f/255, 50f/255, 50f / 255));
                break;
            case RequiredPrimary.Shield:
                DisplayText("Requires a Shield Rune", 14, new Color(50f / 255, 50f / 255, 50f / 255));
                break;
        }
        DisplayText(rune.GetDescription(), 14, Color.black);
    }

    private void DisplaySpellInfo(Spell spell)
    {
        DisplayText("Mana Cost: " + spell.manaCost.ToString(), 14, new Color(0, 30f / 255, 116f / 255));
        string descriptionText = "";
        if (characterInstance != null)
            descriptionText = spell.GetDescription(characterInstance.GetStatBundle());
        else if (playerStats != null)
            descriptionText = spell.GetDescription(playerStats.GetStatBundle());
        else
            descriptionText = spell.GetDescription();
        DisplayText(descriptionText, 14, Color.black);
    }

    private void DisplayEquipmentInfo(EquipmentPiece equipmentPiece)
    {
        DisplayText(equipmentPiece.GetTitle(), 18, Color.black);
        DisplayText("Equipment Level: " + equipmentPiece.ownedEquipmentData.currentLevel, 14, new Color(50f / 255, 0, 0));
        DisplayText(equipmentPiece.GetDescription(), 14, Color.black);
    }

    private void DisplayCharacterInstanceInfo(CharacterInstance characterInstance)
    {
        DisplayText(characterInstance.characterName, 18, Color.black);
        if (characterInstance.statusEffects.Count == 0)
        {
            DisplayText("No active status effects.", 14, Color.black);
        }
        else
        {
            foreach (StatusEffect statusEffect in characterInstance.statusEffects)
            {
                DisplayText(statusEffect.GetTitle() + " : " + statusEffect.turnsRemaining.ToString() + " Turns Remaining", 14, Color.black);
            }
        }
    }
}
