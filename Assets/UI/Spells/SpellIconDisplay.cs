using System.Collections;
using System.Collections.Generic;
using Assets.Inventory.Spells;
using UnityEngine;
using UnityEngine.UI;

public class SpellIconDisplay : MonoBehaviour
{
    [SerializeField] private SpellIconDatabase spellIconDatabase;
    [SerializeField] private Image spellIconImage;
    [SerializeField] private SpellPreview spellPreview;
   private void Start()
    {
        UpdateSpellIcon();
    }
    public void UpdateSpellIcon()
    {
        spellIconImage.sprite = spellIconDatabase.GetSpellIcon(spellPreview.spellIconIndex);
    }
}
