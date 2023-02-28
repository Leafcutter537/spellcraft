using UnityEngine;
using Assets.Combat;
using UnityEngine.SceneManagement;

public class EnemyInteractablePanel : MonoBehaviour
{
    private EnemyStats enemyStats;
    [SerializeField] private CurrentEnemy currentEnemy;
    [SerializeField] private EnemyDetailsPanel enemyDetailsPanel;
    public void SetEnemyStatInfo(EnemyStats enemyStats)
    {
        this.enemyStats = enemyStats;
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
