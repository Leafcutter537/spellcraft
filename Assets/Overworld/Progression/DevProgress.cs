using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(DevProgress), menuName = "ScriptableObjects/DevProgress")]
public class DevProgress : ScriptableObject
{
    public List<DefeatedEnemy> firstDefeatEnemies;
    public Vector2Int playerPosition;
}
