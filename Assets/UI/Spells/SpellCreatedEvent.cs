using Assets.EventSystem;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(SpellCreatedEvent), menuName = "Events/SpellCreatedEvent")]
public class SpellCreatedEvent : BaseEvent<EventParameters>
{

}