using System.Collections;
using System.Collections.Generic;
using Assets.EventSystem;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class RemoveOnQuestState : MonoBehaviour
{
    [SerializeField] private QuestState questState;
    [SerializeField] private string quest;
    [SerializeField] private QuestStatesChangedEvent questStateChangedEvent;
    void Start()
    {
        RemoveIfStateAligned();
    }
    private void OnEnable()
    {
        if (questStateChangedEvent != null)
        {
            questStateChangedEvent.AddListener(OnQuestStateChanged);
        }
    }
    private void OnDisable()
    {
        if (questStateChangedEvent != null)
        {
            questStateChangedEvent.RemoveListener(OnQuestStateChanged);
        }
    }
    private void RemoveIfStateAligned()
    {
        if (QuestLog.GetQuestState(quest) == questState)
        {
            Destroy(gameObject);
        }
    }
    private void OnQuestStateChanged(object sender, EventParameters args)
    {
        RemoveIfStateAligned();
    }
}
