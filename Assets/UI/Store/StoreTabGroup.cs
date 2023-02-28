using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreTabGroup : BasicTabGroup
{
    [SerializeField] private List<SelectPanel> selectPanels;

    public override void SelectTab(int selected)
    {
        HideAll();
        panels[selected].SetActive(true);
        if (selectPanels[selected] != null)
            selectPanels[selected].RefreshInventory();
    }

}
