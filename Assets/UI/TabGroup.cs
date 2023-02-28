using System.Collections.Generic;
using UnityEngine;

public abstract class TabGroup : MonoBehaviour
{
    public List<TabButtonUI> tabButtons;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color selectedColor;
    protected TabButtonUI selectedButton;

    public void Subscribe(TabButtonUI button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButtonUI>();
        }

        tabButtons.Add(button);
        ClearTabs();
    }

    public void OnTabEnter(TabButtonUI button)
    {
        ClearTabs();
        if (button != selectedButton)
        {
            button.background.color = hoverColor;
        }
    }

    public void OnTabExit(TabButtonUI button)
    {
        ClearTabs();
    }

    public void OnTabClick(TabButtonUI button)
    {
        selectedButton = button;
        ClearTabs();
        int index = button.transform.GetSiblingIndex();
        SelectTab(index);
    }

    public void ClearAll()
    {
        selectedButton = null;
        ClearTabs();
    }

    public abstract void SelectTab(int selected);



    private void ClearTabs()
    {
        foreach (TabButtonUI button in tabButtons)
        {
            button.background.color = defaultColor;
        }
        if (selectedButton != null)
        {
            selectedButton.background.color = selectedColor;
        }
    }

}
