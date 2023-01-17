using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SelectChoice
{
    public Sprite icon;
    public string title;
    public string description;

    public virtual string GetTitle()
    {
        return title;
    }

    public virtual string GetDescription()
    {
        return description;
    }


}
