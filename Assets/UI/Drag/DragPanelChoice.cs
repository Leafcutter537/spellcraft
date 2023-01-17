using System.Collections;
using System.Collections.Generic;
using Assets.EventSystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragPanelChoice : SelectPanelChoice, IBeginDragHandler, IEndDragHandler, IDragHandler
{ 
    [Header("Events")]
    [SerializeField] protected StartDragEvent startDragEvent;
    [SerializeField] protected EndDragEvent endDragEvent;
    [Header("Drag Logic")]
    protected bool isHoveringThis;

    protected virtual void OnEnable()
    {
        endDragEvent.AddListener(OnEndDragEvent);
    }
    protected virtual void OnDisable()
    {
        endDragEvent.RemoveListener(OnEndDragEvent);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        isHoveringThis = true;
        base.OnPointerEnter(eventData);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        isHoveringThis = false;
        base.OnPointerExit(eventData);
    }
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        ClearIcon();
        startDragEvent.Raise(this, null);
    }
    public virtual void OnEndDrag(PointerEventData eventData)
    {
        UpdateIcon();
        endDragEvent.Raise(this, null);
    }
    protected virtual void OnEndDragEvent(object sender, EventParameters args)
    {
        if (isHoveringThis)
        {
            if (!ReferenceEquals(sender, this))
            {

            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
    }
}
