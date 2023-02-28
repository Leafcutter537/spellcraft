using System.Collections;
using System.Collections.Generic;
using Assets.EventSystem;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class DragReceiveArea : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool isHoveringThis;
    [SerializeField] private EndDragEvent endDragEvent;

    private void OnEnable()
    {
        endDragEvent.AddListener(OnEndDragEvent);
    }
    private void OnDisable()
    {
        endDragEvent.RemoveListener(OnEndDragEvent);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHoveringThis = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        isHoveringThis = false;
    }
    protected virtual void OnEndDragEvent(object sender, EventParameters args)
    {
        if (isHoveringThis)
        {
            ReceiveDraggedObject(sender, args);
        }
    }
    protected abstract void ReceiveDraggedObject(object sender, EventParameters args);
}
