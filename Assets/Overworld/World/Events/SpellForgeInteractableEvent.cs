using Assets.EventSystem;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(SpellForgeInteractableEvent), menuName = "Events/SpellForgeInteractableEvent")]
public class SpellForgeInteractableEvent : BaseEvent<CombatLogEventParameters>
{
}
