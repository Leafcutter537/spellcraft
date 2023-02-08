using Assets.EventSystem;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(StartCombatAnimationEvent), menuName = "Events/StartCombatAnimationEvent")]
public class StartCombatAnimationEvent : BaseEvent<CombatLogEventParameters>
{
}
