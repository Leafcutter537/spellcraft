using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public Vector2Int cellCoordinates;
    [SerializeField] private int interactRange;
    [SerializeField] private WorldMap worldMap;
    private void Start()
    {
        worldMap.SubscribeInteractable(this);
    }

    public bool IsInInteractRange(Vector3Int charCoords)
    {
        Vector2Int characterCoordinates = new Vector2Int(charCoords.x, charCoords.y);
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
