using System.Collections;
using System.Collections.Generic;
using Assets.EventSystem;
using Assets.Inventory.Runes;
using Assets.Inventory.Scrolls;
using UnityEngine;

public class SpellPreview : MonoBehaviour
{
    [SerializeField] private SpellInfoDisplay spellInfoDisplay;
    [SerializeField] private SpellGenerator spellGenerator;
    [SerializeField] private RuneInScrollChangedEvent runeInScrollChangedEvent;
    public Spell previewedSpell;
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
}
