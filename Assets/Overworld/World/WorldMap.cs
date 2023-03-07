using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldMap : MonoBehaviour
{
    [Header("Tile Info")]
    [SerializeField] private List<TileBase> blockingTiles;
    [SerializeField] private Tilemap tilemap;
    // Object and interactable info
    private Dictionary<Vector2Int, Obstacle> obstacles;
    private List<Interactable> interactables;
    [Header("Event References")]
    [SerializeField] private InteractablePanelsController interactablePanelsController;

    private void Awake()
    {
        obstacles = new Dictionary<Vector2Int, Obstacle>();
        interactables = new List<Interactable>();
    }
    public void AddObstacle(Obstacle obstacle)
    {
        obstacles.Add(obstacle.cellCoordinates, obstacle);
    }
    public void RemoveObstacle(Obstacle obstacle)
    {
        obstacles.Remove(obstacle.cellCoordinates);
    }
    public void SubscribeInteractable(Interactable interactable)
    {
        interactables.Add(interactable);
    }
    public void RemoveInteractable(Interactable interactable)
    {
        interactables.Remove(interactable);
        interactablePanelsController.HideAll();
    }
    public bool IsCellBlocked(Vector2Int cellCoordinates)
    {
        TileBase tile = tilemap.GetTile((Vector3Int)cellCoordinates);
        if (blockingTiles.Contains(tile))
            return true;
        return CheckObstacles(cellCoordinates);
    }
    private bool CheckObstacles(Vector2Int cellCoordinates)
    {
        if (obstacles.TryGetValue(cellCoordinates, out Obstacle obstacle))
        {
            return (obstacle != null);
        }
        return false;
    }
    public void ArrivateAtCell(Vector2Int cellCoordinates)
    {
        CheckInteractables(cellCoordinates);
    }
    private void CheckInteractables(Vector2Int cellCoordinates)
    {
        foreach (Interactable interactable in interactables)
        {
            if (interactable.IsInInteractRange(cellCoordinates))
                interactable.ShowInteractPanel();
        }
    }
}
