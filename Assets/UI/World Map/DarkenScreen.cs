using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class DarkenScreen : MonoBehaviour
{
    [SerializeField] private DarkenScreenController controller;
    [SerializeField] private Animator animator;
    public GameObject toDisappear;
    [SerializeField] private FinishDarkenScreenEvent finishDarkenScreenEvent;
    public void StartDarkenScreen(GameObject toDisappear)
    {
        this.toDisappear = toDisappear;
        animator.SetTrigger("Darken");
    }

    public void DisappearGameObjects()
    {
        toDisappear.SetActive(false);
    }

    private void FinishDarkenScreen()
    {
        finishDarkenScreenEvent.Raise(this, null);
        gameObject.SetActive(false);
    }
}
