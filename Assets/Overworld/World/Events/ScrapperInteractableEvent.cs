using Assets.EventSystem;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(ScrapperInteractableEvent), menuName = "Events/Map/ScrapperInteractableEvent")]
public class ScrapperInteractableEvent : BaseEvent<CombatLogEventParameters>
{
}
