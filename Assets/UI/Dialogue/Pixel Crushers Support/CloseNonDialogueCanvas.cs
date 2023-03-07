using System.Collections;
using System.Collections.Generic;
using Assets.EventSystem;
using UnityEngine;

public class CloseNonDialogueCanvas : MonoBehaviour
{
    public Canvas canvas;
    [SerializeField] private OpenNonDialogueCanvasEvent conversationEndEvent;
    [SerializeField] private CloseNonDialogueCanvasEvent conversationStartEvent;

    private void OnEnable()
    {
        conversationStartEvent.AddListener(OnConversationStart);
        conversationEndEvent.AddListener(OnConversationEnd);
    }
    private void OnDisable()
    {
        conversationStartEvent.RemoveListener(OnConversationStart);
        conversationEndEvent.RemoveListener(OnConversationEnd);
    }
    public void OnConversationStart(object sender, EventParameters args)
    {
        CloseCanvas();
    }
    public void OnConversationEnd(object sender, EventParameters args)
    {
        OpenCanvas();
    }
    public void CloseCanvas()
    {
        canvas.gameObject.SetActive(false);
    }
    public void OpenCanvas()
    {
        canvas.gameObject.SetActive(true);
    }
}
