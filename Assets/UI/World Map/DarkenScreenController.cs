using System.Collections;
using System.Collections.Generic;
using Assets.EventSystem;
using UnityEngine;

public class DarkenScreenController : MonoBehaviour
{
    [SerializeField] private StartDarkenScreenEvent startDarkenScreenEvent;
    [SerializeField] private DarkenScreen darkenScreen;
    private bool startDarkeningScreen;
    [SerializeField] private Canvas canvas;
    private GameObject toDisappear;
    private void Update()
    {
        if (startDarkeningScreen)
        {
            if (canvas.isActiveAndEnabled)
            {
                darkenScreen.gameObject.SetActive(true);
                darkenScreen.StartDarkenScreen(toDisappear);
                startDarkeningScreen = false;
            }
        }
    }
    private void OnEnable()
    {
        startDarkenScreenEvent.AddListener(OnStartDarkenScreen);
    }
    private void OnDisable()
    {
        startDarkenScreenEvent.RemoveListener(OnStartDarkenScreen);
    }
    public void OnStartDarkenScreen(object sender, EventParameters args)
    {
        StartDarkenScreenEventParameters parameters = (StartDarkenScreenEventParameters)args;
        darkenScreen.gameObject.SetActive(true);
        darkenScreen.StartDarkenScreen(parameters.gameObjectToDisappear);
    }
    public void OnStartDarkenScreen(GameObject toDisappear)
    {
        startDarkeningScreen = true;
        this.toDisappear = toDisappear;
    }
}
