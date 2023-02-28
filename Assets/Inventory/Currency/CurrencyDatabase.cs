using System.Collections.Generic;
using UnityEngine;

namespace Assets.Currency
{
    [CreateAssetMenu(fileName = nameof(CurrencyDatabase), menuName = "ScriptableObjects/Currency/CurrencyDatabase")]
    public class CurrencyDatabase : ScriptableObject
    {
        [SerializeField] private List<CurrencyInfo> currencyInfo;

        public CurrencyInfo GetCurrencyInfo(CurrencyType type)
        {
            return currencyInfo[(int)type];
        }
        public string GetCurrencyString(CurrencyQuantity currencyQuantity)
        {
            CurrencyInfo thisCurrencyInfo = currencyInfo[(int)currencyQuantity.currencyType];
            return currencyQuantity.quantity.ToString() + " " + thisCurrencyInfo.currencyName;
        }
    }

    public enum CurrencyType
    {
        WorldOneRuneCurrency,
        InitiateUpgradeCurrency,
        InvokerUpgradeCurrency
    }
}
