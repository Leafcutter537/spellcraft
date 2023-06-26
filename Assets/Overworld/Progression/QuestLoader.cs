using System.Collections;
using System.Collections.Generic;
using Assets.Progression;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class QuestLoader : MonoBehaviour
{
    [SerializeField] private ProgressTracker progressTracker;
    private static bool hasLoadedProgress;

    private void Awake()
    {
        if (progressTracker.loadDevProgress & !hasLoadedProgress & Application.isEditor)
        {
        }
        else if (!hasLoadedProgress)
        {
            if (SaveManager.HasSaveData())
            {
                // Load Save
            }
            else
            {
                string[] allQuests = QuestLog.GetAllQuests(QuestState.Success | QuestState.Active);
                foreach (string quest in allQuests)
                {
                    QuestLog.SetQuestState(quest, QuestState.Unassigned);
                }
                hasLoadedProgress = true;
            }
        }
    }
}