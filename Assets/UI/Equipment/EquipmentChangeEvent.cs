using Assets.EventSystem;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(EquipmentChangeEvent), menuName = "Events/Equipment/EquipmentChangeEvent")]
public class EquipmentChangeEvent : BaseEvent<EventParameters>
{
}
