using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTabGroup : TabGroup
{
    [SerializeField] protected List<GameObject> panels;

    private void Start()
    {
        
    }
    public override void SelectTab(int selected)
    {
        HideAll();
        panels[selected].SetActive(true);
    }

    protected void HideAll()
    {
        foreach (GameObject obj in panels)
        {
            obj.SetActive(false);
        }
    }
}
