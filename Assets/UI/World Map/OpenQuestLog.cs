using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class OpenQuestLog : MonoBehaviour
{
    private StandardUIQuestLogWindow logWindow;
    [SerializeField] private CloseNonDialogueCanvas closeCanvas;
    private void Start()
    {
        logWindow = FindObjectOfType<StandardUIQuestLogWindow>();
    }

    public void Open()
    {
        closeCanvas.CloseCanvas();
        logWindow.Open();
    }

    public void Close()
    {
        closeCanvas.OpenCanvas();
        logWindow.Close();
    }
}
