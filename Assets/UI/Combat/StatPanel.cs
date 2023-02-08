
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class StatPanel : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Image healthFill;
    [SerializeField] private TextMeshProUGUI manaText;
    [SerializeField] private Image manaFill;

    public void ShowStatInfo(string characterName, int currentHP, int maxHP, int currentMP, int maxMP)
    {
        nameText.text = characterName;
        healthText.text = "Health: " + currentHP + " / " + maxHP;
        FillBar(healthFill, currentHP, maxHP);
        manaText.text = "Mana: " + currentMP + " / " + maxMP;
        FillBar(manaFill, currentMP, maxMP);
    }

    public abstract void ShowStatInfo();

    private void FillBar(Image image, int currentStat, int maxStat)
    {
        image.fillAmount = (float) currentStat / (float) maxStat;
    }
}
