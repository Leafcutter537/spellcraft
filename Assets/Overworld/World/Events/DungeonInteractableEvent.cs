using Assets.EventSystem;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(DungeonInteractableEvent), menuName = "Events/Map/DungeonInteractableEvent")]
public class DungeonInteractableEvent : BaseEvent<CombatLogEventParameters>
{
}
