using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class MyLuaFunctions : MonoBehaviour
{
    [SerializeField] private List<GameObject> disappearables;
    [SerializeField] private DarkenScreenController darkenScreenController;
    [SerializeField] private QuestStatesChangedEvent questStatesChangedEvent;

    void OnEnable()
    {
        Lua.RegisterFunction("DisappearGameObject", this, typeof(MyLuaFunctions).GetMethod("DisappearGameObject"));
        Lua.RegisterFunction("RaiseQuestStatesChangedEvent", this, typeof(MyLuaFunctions).GetMethod("RaiseQuestStatesChangedEvent"));
    }

    void OnDisable()
    {
        Lua.UnregisterFunction("DisappearGameObject");
        Lua.UnregisterFunction("RaiseQuestStatesChangedEvent");
    }

    public void DisappearGameObject(string name)
    {
        foreach (GameObject gameObject in disappearables)
        {
            if (gameObject == null)
                continue;
            if (gameObject.name == name)
            {
                darkenScreenController.OnStartDarkenScreen(gameObject);
                // startDarkenScreenEvent.Raise(this, new StartDarkenScreenEventParameters(gameObject));
                return;
            }
        }
    }

    public void RaiseQuestStatesChangedEvent()
    {
        questStatesChangedEvent.Raise(this, null);
    }
}