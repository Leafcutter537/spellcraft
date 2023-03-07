using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonDialogueCanvasEventRaiser : MonoBehaviour
{
    [SerializeField] private CloseNonDialogueCanvasEvent closeNonDialogueCanvasEvent;
    [SerializeField] private OpenNonDialogueCanvasEvent openNonDialogueCanvasEvent;

    public void RaiseCloseNonDialogueCanvasEvent()
    {
        closeNonDialogueCanvasEvent.Raise(this, null);
    }
    public void RaiseOpenNonDialogueCanvasEvent()
    {
        openNonDialogueCanvasEvent.Raise(this, null);
    }
}
