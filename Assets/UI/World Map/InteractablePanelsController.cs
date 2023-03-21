using System.Collections;
using System.Collections.Generic;
using Assets.EventSystem;
using TMPro;
using UnityEditor;
using UnityEngine;

public class InteractablePanelsController : MonoBehaviour
{
    [SerializeField] private List<GameObject> interactablePanels;
    [Header("Leave Cell Event")]
    [SerializeField] private LeaveCellEvent leaveCellEvent;

    private void OnEnable()
    {
        leaveCellEvent.AddListener(OnLeaveCell);
    }
    private void OnDisable()
    {
        leaveCellEvent.RemoveListener(OnLeaveCell);
    }
    public void HideAll()
    {
        foreach (GameObject panel in interactablePanels)
        {
            if (panel != null)
                panel.SetActive(false);
        }
    }
    private void OnLeaveCell(object sender, EventParameters args)
    {
        HideAll();
    }
}
