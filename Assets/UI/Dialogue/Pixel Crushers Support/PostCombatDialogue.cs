using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostCombatDialogue : MonoBehaviour
{
    [SerializeField] private ProgressTracker progressTracker;
    private void Awake()
    {
        if (progressTracker.justDefeatedEnemy == true)
        {
            progressTracker.justDefeatedEnemy = false;
        }
    }
}
