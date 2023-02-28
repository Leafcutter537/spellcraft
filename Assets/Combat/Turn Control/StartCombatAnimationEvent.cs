using Assets.EventSystem;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(StartCombatAnimationEvent), menuName = "Events/Combat/StartCombatAnimationEvent")]
public class StartCombatAnimationEvent : BaseEvent<CombatLogEventParameters>
{
}
