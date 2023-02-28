using Assets.EventSystem;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(CurrencyChangedEvent), menuName = "Events/Currency/CurrencyChangedEvent")]
public class CurrencyChangedEvent : BaseEvent<EventParameters>
{
}
