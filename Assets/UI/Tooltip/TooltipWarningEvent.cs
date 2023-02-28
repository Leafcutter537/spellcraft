using Assets.EventSystem;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(TooltipWarningEvent), menuName = "Events/Tooltip/TooltipWarningEvent")]
public class TooltipWarningEvent : BaseEvent<TooltipWarningEventParameters>
{

}