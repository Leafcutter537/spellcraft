using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TabButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public TabGroup tabGroup;
    public Image background;
    public bool initialSelection;

    private void Awake()
    {
        tabGroup.Subscribe(this);
    }
    private void Start()
    {
        if (initialSelection)
            tabGroup.OnTabClick(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnTabClick(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroup.OnTabExit(this);
    }
}
