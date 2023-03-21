using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class SelectPanel : MonoBehaviour
{
    [Header("Selection")]
    protected List<SelectPanelChoice> selectPanelChoices;
    public List<SelectChoice> itemList;
    protected int selectedIndex;
    private int inventoryIndex;
    [Header("Visuals")]
    [SerializeField] protected Color defaultColor;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Sprite blankSprite;
    [Header("Page Selection")]
    [SerializeField] private Button pageUpButton;
    [SerializeField] private Button pageDownButton;
    [SerializeField] private TextMeshProUGUI pageText;


    protected virtual void Awake()
    {
        selectedIndex = -1;
    }

    protected virtual void Start()
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
        inventoryIndex = 0;
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
        if (pageUpButton != null)
            pageUpButton.interactable = (inventoryIndex + selectPanelChoices.Count < itemList.Count);
        if (pageDownButton != null)
            pageDownButton.interactable = (inventoryIndex - selectPanelChoices.Count >= 0);
        if (pageText != null)
        {
            int numPages = (int)((itemList.Count-1) / selectPanelChoices.Count) + 1;
            int currentPage = (int)(inventoryIndex / selectPanelChoices.Count) + 1;
            pageText.text = "Page " + currentPage + " of " + numPages;
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

    public int GetChoiceIndex(SelectPanelChoice selectPanelChoice)
    {
        return selectPanelChoices.IndexOf(selectPanelChoice);
    }

}
