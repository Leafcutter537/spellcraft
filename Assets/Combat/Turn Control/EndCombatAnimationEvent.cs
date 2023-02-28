using Assets.EventSystem;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(EndCombatAnimationEvent), menuName = "Events/Combat/EndCombatAnimationEvent")]
public class EndCombatAnimationEvent : BaseEvent<CombatLogEventParameters>
{
}
