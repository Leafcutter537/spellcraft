using System.Collections;
using System.Collections.Generic;
using Assets.Combat;
using UnityEngine;
using TMPro;

public class EnemyDetailsPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI statsText;

    public void ShowEnemyDetails(EnemyStats enemyStats)
    {
        titleText.text = enemyStats.enemyName;
        statsText.text = "HP: " + enemyStats.maxHP;
        statsText.text = "Resilience: " + enemyStats.resilience;
    }
}
