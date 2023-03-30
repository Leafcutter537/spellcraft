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
        mouseEnterPathEvent.AddListener(OnMouseEnterSquare);
        mouseExitPathEvent.AddListener(OnMouseExitPath);
    }
    private void OnDisable()
    {
        enterTooltipEvent.RemoveListener(OnEnterTooltip);
        exitTooltipEvent.RemoveListener(OnExitTooltip);
        mouseEnterPathEvent.RemoveListener(OnMouseEnterSquare);
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
            DisplaySpellInfo(spell, selectPanelChoice);
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

    private void OnMouseEnterSquare(object sender, EventParameters args)
    {
        GridSquare grid = sender as GridSquare;
        if (grid.playerProjectile != null)
        {
            DisplayText(grid.playerProjectile.GetProjectileDescription(), 14, Color.black);
        }
        if (grid.enemyProjectile != null)
        {
            DisplayText(grid.enemyProjectile.GetProjectileDescription(), 14, Color.black);
        }
        if (grid.shield != null)
        {
            DisplayText(grid.shield.GetShieldDescription(), 14, Color.black);
        }
        if (tooltipTextObjects.Count == 0)
        {
            DisplayText("Empty", 14, Color.black);
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

    private void DisplaySpellInfo(Spell spell, SelectPanelChoice selectPanelChoice)
    {
        if (characterInstance != null)
        {
            int spellIndex = selectPanelChoice.selectPanel.GetChoiceIndex(selectPanelChoice);
            int cooldown = (characterInstance as PlayerInstance).GetSpellCooldown(spellIndex);
            if (cooldown > 0)
            {
                string plural = cooldown > 1 ? "S" : "";
                DisplayText("ON COOLDOWN: " + cooldown.ToString() + " TURN" + plural + " REMAINING.", 14, new Color(100f / 255, 0, 0));
            }
        }
        DisplayText("Mana Cost: " + spell.manaCost.ToString(), 14, new Color(0, 30f / 255, 116f / 255));
        if (spell.cooldown > 0)
            DisplayText("Cooldown: " + spell.cooldown.ToString(), 14, new Color(30f / 255, 30f / 255, 0));
        if (spell.chargeTime > 0)
            DisplayText("Charge Time: " + spell.chargeTime.ToString(), 14, new Color(30f / 255, 30f / 255, 0));
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
