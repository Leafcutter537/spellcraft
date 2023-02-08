using Assets.EventSystem;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(EndCombatAnimationEvent), menuName = "Events/EndCombatAnimationEvent")]
public class EndCombatAnimationEvent : BaseEvent<CombatLogEventParameters>
{
}
