using System.Collections;
using System.Collections.Generic;
using Assets.Inventory.Spells;
using UnityEngine;

public class SpellIconSelectPanel : SelectPanel
{
    [SerializeField] private SpellIconDatabase spellIconDatabase;
    [SerializeField] private SpellPreview spellPreview;
    protected override void GetInventory()
    {
        itemList = spellIconDatabase.GetItemList();
    }

    protected override void SelectUpdate()
    {
        spellPreview.ChangeSpellIconIndex(GetIndex());
        gameObject.SetActive(false);
    }

}
