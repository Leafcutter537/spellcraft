using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyInteractablePanel : MonoBehaviour
{
    private EnemyStats enemyStats;
    [SerializeField] private CurrentEnemy currentEnemy;
    public void SetEnemyStatInfo(EnemyStats enemyStats)
    {
        this.enemyStats = enemyStats;
    }
    public void StartFight()
    {
        currentEnemy.enemyStats = enemyStats;
        SceneManager.LoadScene("CombatScene");
    }
}
