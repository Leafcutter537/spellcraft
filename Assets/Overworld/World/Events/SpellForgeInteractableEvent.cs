using Assets.EventSystem;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(SpellForgeInteractableEvent), menuName = "Events/Map/SpellForgeInteractableEvent")]
public class SpellForgeInteractableEvent : BaseEvent<CombatLogEventParameters>
{
}
