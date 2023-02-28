using Assets.EventSystem;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(ShopInteractableEvent), menuName = "Events/Map/ShopInteractableEvent")]
public class ShopInteractableEvent : BaseEvent<CombatLogEventParameters>
{
}
