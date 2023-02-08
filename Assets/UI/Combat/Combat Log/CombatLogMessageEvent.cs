using Assets.EventSystem;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(CombatLogMessageEvent), menuName = "Events/CombatLogMessageEvent")]
public class CombatLogMessageEvent : BaseEvent<CombatLogEventParameters>
{
}
