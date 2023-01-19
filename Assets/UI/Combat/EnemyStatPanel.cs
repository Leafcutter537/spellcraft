
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemyName;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private EnemyInstance enemyInstance;

    public void ShowEnemyInfo()
    {
        enemyName.text = enemyInstance.enemyStats.enemyName;
        healthText.text = "Health: " + enemyInstance.currentHealth + " / " + enemyInstance.enemyStats.maxHP;
    }
}
