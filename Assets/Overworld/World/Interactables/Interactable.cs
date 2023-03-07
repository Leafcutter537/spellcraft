using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public Vector2Int cellCoordinates;
    [SerializeField] private int interactRange;
    [SerializeField] private WorldMap worldMap;
    protected virtual void Start()
    {
        worldMap.SubscribeInteractable(this);
    }
    private void OnDestroy()
    {
        worldMap.RemoveInteractable(this);
    }

    public bool IsInInteractRange(Vector2Int characterCoordinates)
    {
        if (interactRange == 0)
        {
            return (cellCoordinates == characterCoordinates);
        }
        else
        {
            if (cellCoordinates.x == characterCoordinates.x & Mathf.Abs(cellCoordinates.y - characterCoordinates.y) <= 1)
                return true;
            if (cellCoordinates.y == characterCoordinates.y & Mathf.Abs(cellCoordinates.x - characterCoordinates.x) <= 1)
                return true;
            return false;
        }
    }
    public abstract void ShowInteractPanel();

}
