using System.Collections;
using System.Collections.Generic;
using Assets.EventSystem;
using UnityEngine;

public class FollowCursorHiddenController : MonoBehaviour
{
    [Header("Event References")]
    [SerializeField] private EnterTooltipEvent enterTooltipEvent;
    [SerializeField] private ExitTooltipEvent exitTooltipEvent;
    [SerializeField] private StartDragEvent startDragEvent;
    [SerializeField] private EndDragEvent endDragEvent;
    [SerializeField] private TooltipWarningEvent tooltipWarningEvent;
    [Header("Monobehavior References")]
    [SerializeField] private FollowCursor tooltip;
    [SerializeField] private FollowCursor dragImage;
    [SerializeField] private FollowCursor warning;
    private bool isHovering;
    private bool isDragging;
    [Header("Tooltip Warnings")]
    [SerializeField] private float warningDuration;
    private float timeSinceWarning;
    private void Awake()
    {
        timeSinceWarning = 1000f;
    }
    private void OnEnable()
    {
        enterTooltipEvent.AddListener(OnEnterTooltip);
        exitTooltipEvent.AddListener(OnExitTooltip);
        endDragEvent.AddListener(OnEndDragEvent);
        startDragEvent.AddListener(OnStartDragEvent);
        tooltipWarningEvent.AddListener(OnTooltipWarning);
    }
    private void OnDisable()
    {
        enterTooltipEvent.RemoveListener(OnEnterTooltip);
        exitTooltipEvent.RemoveListener(OnExitTooltip);
        endDragEvent.RemoveListener(OnEndDragEvent);
        startDragEvent.RemoveListener(OnStartDragEvent);
        tooltipWarningEvent.RemoveListener(OnTooltipWarning);
    }
    private void Update()
    {
        if (timeSinceWarning < warningDuration)
        {
            timeSinceWarning += Time.deltaTime;
            SetWarningHidden(false);
            SetTooltipHidden(true);
            SetDragImageHidden(true);
            return;
        }
        SetWarningHidden(true);
        if (isDragging)
        {
            SetDragImageHidden(false);
            SetTooltipHidden(true);
        }
        else if (isHovering)
        {
            SetDragImageHidden(true);
            SetTooltipHidden(false);
        }
        else
        {
            SetDragImageHidden(true);
            SetTooltipHidden(true);
        }
    }
    private void SetTooltipHidden(bool isHidden)
    {
        if (tooltip != null)
        {
            tooltip.isHidden = isHidden;
        }
    }
    private void SetDragImageHidden(bool isHidden)
    {
        if (dragImage != null)
        {
            dragImage.isHidden = isHidden;
        }
    }
    private void SetWarningHidden(bool isHidden)
    {
        if (warning != null)
        {
            warning.isHidden = isHidden;
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
    private void OnTooltipWarning(object sender, EventParameters args)
    {
        timeSinceWarning = 0;
    }
}
