using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Inventory.Spells;
using TMPro;
using Assets.Currency;
using Assets.Tutorial;

public class CreateSpellPanel : MonoBehaviour
{
    [SerializeField] private Button createSpellButton;
    [SerializeField] private GameObject confirmSpellParent;
    [SerializeField] private TextMeshProUGUI nameSpellTitle;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private GameObject scrollDisplayMask;
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private SpellPreview spellPreview;
    [SerializeField] private SpellCreatedEvent spellCreatedEvent;
    [SerializeField] private SpellforgeTutorialController tutorialController;
    [SerializeField] private TooltipWarningEvent toolTipWarningEvent;

    public void CreateSpell()
    {
        if (spellPreview.previewedSpell == null)
        {
            return;
        }
        if (spellPreview.previewedSpell.spellEffects.Count == 0)
        {
            toolTipWarningEvent.Raise(this, new TooltipWarningEventParameters("You cannot create a spell with no spell effects!"));
            return;
        }
        if (!tutorialController.SpellIsCorrectDuringTutorial())
        {
            return;
        }
        SetSpellNamesActive(true);
        nameInputField.text = GetDefaultName();
    }
    public void CancelCreateSpell()
    {
        SetSpellNamesActive(false);
    }
    public void ConfirmCreateSpell()
    {
        PlayerSpell spell = spellPreview.previewedSpell;
        spell.title = nameInputField.text;
        inventoryController.spells.Add(spell);
        spellCreatedEvent.Raise(this, null);
        inventoryController.RemoveHeldRunesFromInventory();
        inventoryController.SubtractCurrencyQuantity(spell.spellData.scrollData.cost);
        SetSpellNamesActive(false);
        tutorialController.OnSpellCreated();
    }
    private string GetDefaultName()
    {
        int i = 1;
        while (true)
        {
            string spellName = "Spell " + i;
            if (!inventoryController.HasSpellWithName(spellName))
                return spellName;
            i++;
        }
    }
    private void SetSpellNamesActive(bool isActive)
    {
        createSpellButton.gameObject.SetActive(!isActive);
        scrollDisplayMask.SetActive(isActive);
        confirmSpellParent.SetActive(isActive);
    }

}