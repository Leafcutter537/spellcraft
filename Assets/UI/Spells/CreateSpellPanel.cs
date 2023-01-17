using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreateSpellPanel : MonoBehaviour
{
    [SerializeField] private Button createSpellButton;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private TextMeshProUGUI nameSpellTitle;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private SpellPreview spellPreview;
    [SerializeField] private SpellCreatedEvent spellCreatedEvent;

    public void CreateSpell()
    {
        if (spellPreview.previewedSpell == null)
        {
            return;
        }
        if (spellPreview.previewedSpell.spellEffects.Count == 0)
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
        Spell spell = spellPreview.previewedSpell;
        spell.title = nameInputField.text;
        inventoryController.spells.Add(spell);
        spellCreatedEvent.Raise(this, null);
        inventoryController.RemoveHeldRunesFromInventory();
        SetSpellNamesActive(false);
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
        confirmButton.gameObject.SetActive(isActive);
        cancelButton.gameObject.SetActive(isActive);
        nameInputField.gameObject.SetActive(isActive);
        nameSpellTitle.gameObject.SetActive(isActive);
    }

}