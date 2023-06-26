using Assets.Combat;
using UnityEngine;

public class EnemyStatPanel : StatPanel
{
    [Header("Enemy References")]
    [SerializeField] private EnemyInstance enemyInstance;
    [SerializeField] private EnemyDetailsPanel enemyDetailsPanel;

    public override void ShowStatInfo()
    {
        StatBundle stats = enemyInstance.GetStatBundle();
        ShowStatInfo(enemyInstance.characterName, enemyInstance.currentHP, enemyInstance.maxHP,
            enemyInstance.currentMP, enemyInstance.maxMP, stats.resilience);
    }
    public void ShowEnemyDetails()
    {
        enemyDetailsPanel.gameObject.SetActive(true);
        enemyDetailsPanel.ShowEnemyDetails(enemyInstance.enemyStats);
    }
}
