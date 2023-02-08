using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Assets.EventSystem;
using UnityEngine;

public class ChangeActiveState : MonoBehaviour
{
    [Header("Game Objects Set Active or Inactive")]
    [SerializeField] private List<GameObject> objectsSetActive;
    [SerializeField] private List<GameObject> objectsSetUnactive;
    [Header("Event References")]
    [SerializeField] private BaseEvent<EventParameters> setObjectsActiveEvent;
    [SerializeField] private BaseEvent<EventParameters> setObjectsInactiveEvent;

    private void OnEnable()
    {
        if (setObjectsActiveEvent != null)
            setObjectsActiveEvent.AddListener(SetObjectsActive);
        if (setObjectsInactiveEvent != null)
            setObjectsInactiveEvent.AddListener(SetObjectsUnactive);
    }
    private void OnDisable()
    {
        if (setObjectsActiveEvent != null)
            setObjectsActiveEvent.RemoveListener(SetObjectsActive);
        if (setObjectsInactiveEvent != null)
            setObjectsInactiveEvent.RemoveListener(SetObjectsUnactive);
    }
    public void Activate()
    {
        foreach (GameObject obj in objectsSetActive)
            obj.SetActive(true);
        foreach (GameObject obj in objectsSetUnactive)
            obj.SetActive(false);
    }
    private void SetObjectsActive(object sender, EventParameters args)
    {
        foreach (GameObject obj in objectsSetActive)
            obj.SetActive(true);
    }
    private void SetObjectsUnactive(object sender, EventParameters args)
    {
        foreach (GameObject obj in objectsSetUnactive)
            obj.SetActive(false);
    }
}
