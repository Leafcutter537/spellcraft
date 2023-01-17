using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

public class SelectPanelChoice : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler, IComparable
{

    public enum ShowBehavior
    {
        OnHover,
        OnSelect,
        Always
    }

    public SelectPanel selectPanel;
    public Image panelImage;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] protected TextMeshProUGUI descriptionText;
    [SerializeField] private InfoDisplay infoDisplay;

    [SerializeField] protected EnterTooltipEvent enterTooltipEvent;
    [SerializeField] protected ExitTooltipEvent exitTooltipEvent;

    public ShowBehavior showIcon;
    public ShowBehavior showTitle;
    public ShowBehavior showDescription;
    public ShowBehavior showInfo;


    public SelectChoice selectChoice;
    protected bool isSelected;
    private Color selectedColor;
    private Color defaultColor;
    private Sprite defaultSprite;

    private string originalTitleText;
    private string originalDescriptionText;
    private Sprite originalSprite;


    private void Awake()
    {
        if (selectPanel != null)
        {
            selectPanel.Subscribe(this);
        }
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        SelectThis();
    }

    // Call this when something other than the SelectPanel is triggering a selection.
    public bool SelectThis()
    {
        return selectPanel.SelectSlot(this);
    }

    public int CompareTo(System.Object obj)
    {
        SelectPanelChoice selectPanelChoice = obj as SelectPanelChoice;
        return this.transform.GetSiblingIndex().CompareTo(selectPanelChoice.transform.GetSiblingIndex());
    }

    public virtual void SetChoice(SelectChoice selectChoice)
    {
        this.selectChoice = selectChoice;
        if (selectChoice == null)
        {
            if (showIcon == ShowBehavior.Always)
            {
                ClearIcon();
            }
            if (showDescription == ShowBehavior.Always)
            {
                ClearDescription();
            }
            if (showTitle == ShowBehavior.Always)
            {
                ClearTitle();
            }
            if (showInfo == ShowBehavior.Always)
            {
                ClearInfoDisplay();
            }
            return;
        }
        if (showIcon == ShowBehavior.Always)
        {
            UpdateIcon();
        }
        if (showDescription == ShowBehavior.Always)
        {
            UpdateDescription();
        }
        if (showTitle == ShowBehavior.Always)
        {
            UpdateTitle();
        }
        if (showInfo == ShowBehavior.Always)
        {
            UpdateInfo();
        }
    }

    // Only the SelectPanel should call this.
    public virtual void Select()
    {
        isSelected = true;
        panelImage.color = selectedColor;
        if (showIcon == ShowBehavior.OnSelect)
        {
            UpdateIcon();
        }
        if (showDescription == ShowBehavior.OnSelect)
        {
            UpdateDescription();
        }
        if (showTitle == ShowBehavior.OnSelect)
        {
            UpdateTitle();
        }
        if (showInfo == ShowBehavior.OnSelect)
        {
            UpdateInfo();
        }
    }

    public void Deselect()
    {
        isSelected = false;
        panelImage.color = defaultColor;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (selectChoice != null & enterTooltipEvent != null)
            enterTooltipEvent.Raise(this, null);
        if (showIcon == ShowBehavior.OnHover)
        {
            originalSprite = icon.sprite;
            UpdateIcon();
        }
        if (showDescription == ShowBehavior.OnHover)
        {
            originalDescriptionText = descriptionText.text;
            UpdateDescription();
        }
        if (showTitle == ShowBehavior.OnHover)
        {
            originalTitleText = titleText.text;
            UpdateTitle();
        }
        if (showInfo == ShowBehavior.OnHover)
        {
            UpdateInfo();
        }
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (exitTooltipEvent != null)
        {
            exitTooltipEvent.Raise(this, null);
        }
        if (showIcon == ShowBehavior.OnHover & !isSelected)
        {
            RestoreOriginalIcon();
        }
        if (showDescription == ShowBehavior.OnHover & !isSelected)
        {
            RestoreOriginalDescription();
        }
        if (showTitle == ShowBehavior.OnHover & !isSelected)
        {
            RestoreOriginalTitle();
        }
        if (showInfo == ShowBehavior.OnHover)
        {
            ClearInfoDisplay();
        }
    }

    private void RestoreOriginalIcon()
    {
        if (icon != null)
        {
            icon.sprite = originalSprite;
        }
    }
    private void RestoreOriginalDescription()
    {
        if (descriptionText != null)
        {
            descriptionText.text = originalDescriptionText;
        }
    }
    private void RestoreOriginalTitle()
    {
        if (titleText != null)
        {
            titleText.text = originalTitleText;
        }
    }

    public void SetSelectedColor(Color color)
    {
        selectedColor = color;
    }

    public void SetDefaultColor(Color color)
    {
        defaultColor = color;
    }

    public void SetDefaultSprite(Sprite sprite)
    {
        defaultSprite = sprite;
    }

    protected void UpdateIcon()
    {
        if (icon != null & selectChoice != null)
        {
            icon.sprite = selectChoice.icon;
        }
    }

    protected virtual void UpdateDescription()
    {
        if (descriptionText != null & selectChoice != null)
        {
            descriptionText.text = selectChoice.GetDescription();
        }
    }

    private void UpdateTitle()
    {
        if (titleText != null & selectChoice != null)
        {
            titleText.text = selectChoice.GetTitle();
        }
    }

    private void UpdateInfo()
    {
        if (infoDisplay != null & selectChoice != null)
        {
            infoDisplay.DisplayInfo(selectChoice);
        }
    }


    protected void ClearIcon()
    {
        if (icon != null)
        {
            icon.sprite = defaultSprite;
        }
    }

    private void ClearDescription()
    {
        if (descriptionText != null)
        {
            descriptionText.text = "";
        }
    }

    private void ClearTitle()
    {
        if (titleText != null)
        {
            titleText.text = "";
        }
    }

    private void ClearInfoDisplay()
    {
        if (infoDisplay != null)
        {
            infoDisplay.ClearInfo();
        }
    }

    public virtual void ClearAll()
    {
        ClearDescription();
        ClearIcon();
        ClearTitle();
        ClearInfoDisplay();
    }
}
