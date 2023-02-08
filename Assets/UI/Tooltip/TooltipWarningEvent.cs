using Assets.EventSystem;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(TooltipWarningEvent), menuName = "Events/TooltipWarningEvent")]
public class TooltipWarningEvent : BaseEvent<TooltipWarningEventParameters>
{

}