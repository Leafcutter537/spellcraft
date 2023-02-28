using Assets.EventSystem;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(InsufficientManaEvent), menuName = "Events/Combat/InsufficientManaEvent")]
public class InsufficientManaEvent : BaseEvent<EventParameters>
{

}