using System;
using System.Collections.Generic;

namespace Assets.Currency
{
    [Serializable]
    public class CurrencyQuantity
    {
        public int quantity;
        public CurrencyType currencyType;
        public CurrencyQuantity(int quantity, CurrencyType currencyType)
        {
            this.quantity = quantity;
            this.currencyType = currencyType;
        }

        public static List<CurrencyQuantity> CondenseValues(List<CurrencyQuantity> originalValues)
        {
            List<CurrencyQuantity> newValues = new List<CurrencyQuantity>();
            foreach (CurrencyQuantity originalValue in originalValues)
            {
                bool alreadyExisted = false;
                foreach (CurrencyQuantity newValue in newValues)
                {
                    if (originalValue.currencyType == newValue.currencyType)
                    {
                        newValue.quantity += originalValue.quantity;
                        alreadyExisted = true;
                    }
                }
                if (!alreadyExisted)
                    newValues.Add(originalValue);
            }
            return newValues;
        }
    }
}
