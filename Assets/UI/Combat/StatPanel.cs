
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
    [SerializeField] private TextMeshProUGUI resilienceText;

    public void ShowStatInfo(string characterName, int currentHP, int maxHP, int currentMP, int maxMP, int resilience)
    {
        nameText.text = characterName;
        healthText.text = "Health: " + currentHP + " / " + maxHP;
        FillBar(healthFill, currentHP, maxHP);
        if (manaText != null)
        {
            manaText.text = "Mana: " + currentMP + " / " + maxMP;
            FillBar(manaFill, currentMP, maxMP);
        }
        if (resilienceText != null)
        {
            resilienceText.text = "Resilience: " + resilience;
        }
    }

    public abstract void ShowStatInfo();

    private void FillBar(Image image, int currentStat, int maxStat)
    {
        image.fillAmount = (float) currentStat / (float) maxStat;
    }
}
