using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Currency
{
    [CreateAssetMenu(fileName = nameof(DevCurrency), menuName = "ScriptableObjects/Currency/DevCurrency")]
    public class DevCurrency : ScriptableObject
    {
        public List<CurrencyQuantity> currencyQuantities;
    }
}
