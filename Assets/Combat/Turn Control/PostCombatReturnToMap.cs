using System.Collections;
using System.Collections.Generic;
using Assets.Combat;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PostCombatReturnToMap : MonoBehaviour
{
    [SerializeField] private EnemyInstance enemyInstance;
    [SerializeField] private string worldMapName;
    [SerializeField] private string dungeonName;

    public void ReturnToMap()
    {
        if (enemyInstance.enemyID == -1)
            SceneManager.LoadScene(dungeonName);
        else
            SceneManager.LoadScene(worldMapName);
    }
}
