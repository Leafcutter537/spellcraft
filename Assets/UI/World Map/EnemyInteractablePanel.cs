using UnityEngine;
using Assets.Combat;
using UnityEngine.SceneManagement;
using TMPro;

public class EnemyInteractablePanel : MonoBehaviour
{
    private EnemyStats enemyStats;
    [SerializeField] private CurrentEnemy currentEnemy;
    [SerializeField] private EnemyDetailsPanel enemyDetailsPanel;
    [SerializeField] private TextMeshProUGUI encounterText;
    public void SetEnemyStatInfo(EnemyStats enemyStats)
    {
        this.enemyStats = enemyStats;
        if (encounterText != null)
        {
            encounterText.text = enemyStats.enemyName + " blocks your path!";
        }
    }
    public void StartFight()
    {
        currentEnemy.enemyStats = enemyStats;
        SceneManager.LoadScene("CombatScene");
    }
    public void ShowEnemyDetails()
    {
        enemyDetailsPanel.gameObject.SetActive(true);
        enemyDetailsPanel.ShowEnemyDetails(enemyStats);
        gameObject.SetActive(false);
    }
}
