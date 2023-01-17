using System.Collections;
using System.Collections.Generic;
using Assets.EventSystem;
using UnityEngine;

public class FollowCursorHiddenController : MonoBehaviour
{
    [SerializeField] private EnterTooltipEvent enterTooltipEvent;
    [SerializeField] private ExitTooltipEvent exitTooltipEvent;
    [SerializeField] private StartDragEvent startDragEvent;
    [SerializeField] private EndDragEvent endDragEvent;
    [SerializeField] private FollowCursor tooltip;
    [SerializeField] private FollowCursor dragImage;
    private bool isHovering;
    private bool isDragging;

    private void OnEnable()
    {
        enterTooltipEvent.AddListener(OnEnterTooltip);
        exitTooltipEvent.AddListener(OnExitTooltip);
        endDragEvent.AddListener(OnEndDragEvent);
        startDragEvent.AddListener(OnStartDragEvent);
    }
    private void OnDisable()
    {
        enterTooltipEvent.RemoveListener(OnEnterTooltip);
        exitTooltipEvent.RemoveListener(OnExitTooltip);
        endDragEvent.RemoveListener(OnEndDragEvent);
        startDragEvent.RemoveListener(OnStartDragEvent);
    }
    private void Update()
    {
        if (isDragging)
        {
            dragImage.isHidden = false;
            tooltip.isHidden = true;
        }
        else if (isHovering)
        {
            dragImage.isHidden = true;
            tooltip.isHidden = false;
        }
        else
        {
            dragImage.isHidden = true;
            tooltip.isHidden = true;
        }
    }

    private void OnEnterTooltip(object sender, EventParameters args)
    {
        isHovering = true;
    }
    private void OnExitTooltip(object sender, EventParameters args)
    {
        isHovering = false;
    }
    private void OnStartDragEvent(object sender, EventParameters args)
    {
        isDragging = true;
    }
    private void OnEndDragEvent(object sender, EventParameters args)
    {
        isDragging = false;
    }
}
