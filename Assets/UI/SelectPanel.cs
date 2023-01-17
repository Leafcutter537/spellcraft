using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class SelectPanel : MonoBehaviour
{
    [Header("Selection")]
    protected List<SelectPanelChoice> selectPanelChoices;
    public List<SelectChoice> itemList;
    private int selectedIndex;
    private int inventoryIndex;
    [Header("Visuals")]
    [SerializeField] protected Color defaultColor;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Sprite blankSprite;


    protected virtual void Awake()
    {
        selectedIndex = -1;
    }

    private void Start()
    {
        RefreshInventory();
    }

    public void Subscribe(SelectPanelChoice selectPanelChoice)
    {
        if (selectPanelChoices == null)
        {
            selectPanelChoices = new List<SelectPanelChoice>();
        }

        if (defaultColor == null)
        {
            defaultColor = new Color(0.8f, 0.8f, 0.8f);
        }

        if (selectedColor == null)
        {
            selectedColor = new Color(0.8f, 0, 0);
        }

        selectPanelChoices.Add(selectPanelChoice);
        selectPanelChoices.Sort();
        selectPanelChoice.SetDefaultColor(defaultColor);
        selectPanelChoice.SetSelectedColor(selectedColor);
        selectPanelChoice.SetDefaultSprite(blankSprite);
    }
    public bool SelectSlot(SelectPanelChoice selectPanelChoice)
    {
        int index = selectPanelChoice.transform.GetSiblingIndex();

        if (selectedIndex != -1)
        {
            selectPanelChoices[selectedIndex].Deselect();
        }
        if (index + inventoryIndex < itemList.Count)
        {
            selectedIndex = index;
            selectPanelChoice.Select();
            SelectUpdate();
            return true;
        }
        else
        {
            return false;
        }
    }

    protected virtual void SelectUpdate()
    {

    }

    public void RefreshInventory()
    {
        GetInventory();
        AssignSelectChoices();
    }

    protected abstract void GetInventory();

    public void AssignSelectChoices()
    {
        ClearSelection();
        int i = inventoryIndex;
        int j = 0;
        while (i < itemList.Count & j < selectPanelChoices.Count)
        {
            selectPanelChoices[j].SetChoice(itemList[i]);
            i++;
            j++;
        }

        while (j < selectPanelChoices.Count)
        {
            selectPanelChoices[j].SetChoice(null);
            j++;
        }

    }

    public void MoveUp()
    {
        if (inventoryIndex + selectPanelChoices.Count < itemList.Count)
        {
            ClearSelection();
            inventoryIndex += selectPanelChoices.Count;
        }
        AssignSelectChoices();
    }

    public void MoveDown()
    {
        if (inventoryIndex - selectPanelChoices.Count >= 0)
        {
            ClearSelection();
            inventoryIndex -= selectPanelChoices.Count;
        }
        AssignSelectChoices();
    }


    public void ClearSelection()
    {
        selectedIndex = -1;
        foreach (SelectPanelChoice selectPanelChoice in selectPanelChoices)
        {
            selectPanelChoice.Deselect();
            selectPanelChoice.ClearAll();
        }

    }

    public void Deselect()
    {
        selectedIndex = -1; 
        foreach (SelectPanelChoice selectPanelChoice in selectPanelChoices)
        {
            selectPanelChoice.Deselect();
        }
    }

    public SelectChoice GetSelected()
    {
        if (selectedIndex != -1 & itemList.Count > inventoryIndex + selectedIndex)
            return itemList[inventoryIndex + selectedIndex];
        return null;
    }

    public SelectChoice GetItem(SelectPanelChoice selectPanelChoice)
    {
        int index = selectPanelChoice.transform.GetSiblingIndex();
        if (itemList.Count > inventoryIndex + index)
            return itemList[inventoryIndex + index];
        return null;
    }

    public int GetIndex()
    {
        return inventoryIndex + selectedIndex;
    }

}
