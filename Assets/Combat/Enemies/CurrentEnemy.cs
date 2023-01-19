using UnityEngine;

[CreateAssetMenu(fileName = nameof(CurrentEnemy), menuName = "ScriptableObjects/CurrentEnemy")]
public class CurrentEnemy : ScriptableObject
{
    public EnemyStats enemyStats;
}