using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInstance : MonoBehaviour
{
    [SerializeField] private CurrentEnemy currentEnemy;
    public int currentHealth;
    public EnemyStats enemyStats;
    [SerializeField] private EnemyStatPanel enemyStatPanel;

    private void Awake()
    {
        enemyStats = currentEnemy.enemyStats;
        currentHealth = enemyStats.maxHP;
        enemyStatPanel.ShowEnemyInfo();
    }

}
