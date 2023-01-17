using System.Collections.Generic;
using UnityEngine;

public class ChangeActiveState : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectsSetActive;
    [SerializeField] private List<GameObject> objectsSetUnactive;

    public void Activate()
    {
        foreach (GameObject obj in objectsSetActive)
            obj.SetActive(true);
        foreach (GameObject obj in objectsSetUnactive)
            obj.SetActive(false);
    }
}
