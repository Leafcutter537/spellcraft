using System.Collections;
using System.Collections.Generic;
using Assets.EventSystem;
using Assets.Inventory.Runes;
using Assets.Inventory.Scrolls;
using UnityEngine;
using Assets.Inventory.Spells;

public class SpellPreview : MonoBehaviour
{
    [SerializeField] private SpellInfoDisplay spellInfoDisplay;
    [SerializeField] private SpellGenerator spellGenerator;
    [SerializeField] private RuneInScrollChangedEvent runeInScrollChangedEvent;
    public PlayerSpell previewedSpell;
    private List<RuneSelectPanelChoice> runeSlots;
    private ScrollData scrollData;

    private void OnEnable()
    {
        runeInScrollChangedEvent.AddListener(OnRuneInScrollChanged);
    }
    private void OnDisable()
    {
        runeInScrollChangedEvent.RemoveListener(OnRuneInScrollChanged);
    }
    public void UpdateSpellPreview()
    {
        List<RuneData> runeData = new List<RuneData>();
        foreach (RuneSelectPanelChoice runeSlot in runeSlots)
        {
            if (runeSlot.selectChoice != null)
            {
                Rune rune = runeSlot.selectChoice as Rune;
                runeData.Add(rune.runeData);
            }
            else
            {
                runeData.Add(null);
            }    
        }
        SpellData spellData = new SpellData(scrollData, runeData);
        previewedSpell = spellGenerator.CreateSpell(spellData);
        spellInfoDisplay.DisplaySpellInfo(previewedSpell);
    }
    public void UpdateScroll(ScrollData scrollData, List<RuneSelectPanelChoice> runeSlots)
    {
        this.scrollData = scrollData;
        this.runeSlots = runeSlots;
        previewedSpell = null;
        UpdateSpellPreview();
    }
    private void OnRuneInScrollChanged(object sender, EventParameters args)
    {
        UpdateSpellPreview();
    }
    
    public bool IsRuneEntryValid(RuneSelectPanelChoice targetRuneSlot, RuneData newAddition)
    {
        List<RuneData> runeData = new List<RuneData>();
        foreach (RuneSelectPanelChoice runeSlot in runeSlots)
        {
            if (runeSlot.selectChoice != null & !ReferenceEquals(runeSlot, targetRuneSlot))
            {
                Rune rune = runeSlot.selectChoice as Rune;
                runeData.Add(rune.runeData);
            }
            else if (ReferenceEquals(runeSlot, targetRuneSlot))
            {
                runeData.Add(newAddition);
            }
        }
        return spellGenerator.IsRuneEntryValid(runeData, scrollData);
    }

    public bool IsRuneSwapValid(RuneSelectPanelChoice slotA, RuneSelectPanelChoice slotB)
    {
        List<RuneData> runeData = new List<RuneData>();
        foreach (RuneSelectPanelChoice runeSlot in runeSlots)
        {
            if (runeSlot.selectChoice != null & !ReferenceEquals(runeSlot, slotA) & !ReferenceEquals(runeSlot, slotB))
            {
                Rune rune = runeSlot.selectChoice as Rune;
                runeData.Add(rune.runeData);
            }
            else if (ReferenceEquals(runeSlot, slotA))
            {
                Rune rune = slotB.selectChoice as Rune;
                if (rune != null)
                    runeData.Add(rune.runeData);
                else
                    runeData.Add(null);
            }
            else if (ReferenceEquals(runeSlot, slotB))
            {
                Rune rune = slotA.selectChoice as Rune;
                if (rune != null)
                    runeData.Add(rune.runeData);
                else
                    runeData.Add(null);
            }
            else if (runeSlot.selectChoice == null)
            {
                runeData.Add(null);
            }
        }
        return spellGenerator.IsRuneEntryValid(runeData, scrollData);
    }
}
