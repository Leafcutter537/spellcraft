using System.Collections;
using System.Collections.Generic;
using Assets.EventSystem;
using UnityEngine;
using UnityEngine.Events;

public class InvokeOnBaseEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent methodToInvoke;
    [SerializeField] private BaseEvent<EventParameters> baseEvent;

    private void OnEnable()
    {
        baseEvent.AddListener(OnBaseEventCall);
    }
    private void OnDisable()
    {
        baseEvent.RemoveListener(OnBaseEventCall);
    }

    private void OnBaseEventCall(object sender, EventParameters args)
    {
        StartCoroutine(delayedAction(() => {
            methodToInvoke.Invoke();
        }, 1));
    }

    IEnumerator delayedAction(UnityAction action, int nFrames)
    {
        yield return null;
        for (int i = 0; i < nFrames; i++)
        {
            yield return new WaitForEndOfFrame();
        }
        action.Invoke();
    }
}
