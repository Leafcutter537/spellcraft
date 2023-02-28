using UnityEngine;
using Assets.Combat;

[CreateAssetMenu(fileName = nameof(CurrentEnemy), menuName = "ScriptableObjects/CurrentEnemy")]
public class CurrentEnemy : ScriptableObject
{
    public EnemyStats enemyStats;
}