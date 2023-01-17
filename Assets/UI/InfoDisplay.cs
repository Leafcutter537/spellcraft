using UnityEngine;

public abstract class InfoDisplay : MonoBehaviour
{
    public abstract void DisplayInfo(SelectChoice selectChoice);

    public abstract void ClearInfo();
}
