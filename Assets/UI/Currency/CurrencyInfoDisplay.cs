using Assets.Currency;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyInfoDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private Image currencyImage;
    [SerializeField] private CurrencyDatabase currencyDatabase;

    public void DisplayInfo(CurrencyQuantity currencyQuantity)
    {
        CurrencyInfo currencyInfo = currencyDatabase.GetCurrencyInfo(currencyQuantity.currencyType);
        currencyText.text = currencyQuantity.quantity + " " + currencyInfo.currencyName;
        currencyImage.sprite = currencyInfo.currencySprite;
    }
}
