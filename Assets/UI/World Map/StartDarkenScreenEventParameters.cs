using Assets.EventSystem;
using UnityEngine;

public class StartDarkenScreenEventParameters : EventParameters
{
    public GameObject gameObjectToDisappear;
    public StartDarkenScreenEventParameters(GameObject gameObjectToDisappear)
    {
        this.gameObjectToDisappear = gameObjectToDisappear;
    }
}
