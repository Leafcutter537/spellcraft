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
    private List<Obstacle> obstacles;
    private List<Interactable> interactables;

    private void Awake()
    {
        obstacles = new List<Obstacle>();
        interactables = new List<Interactable>();
    }
    public void SubscribeObstacle(Obstacle obstacle)
    {
        obstacles.Add(obstacle);
    }
    public void SubscribeInteractable(Interactable interactable)
    {
        interactables.Add(interactable);
    }
    public bool IsCellBlocked(Vector3Int cellCoordinates)
    {
        TileBase tile = tilemap.GetTile(cellCoordinates);
        if (blockingTiles.Contains(tile))
            return true;
        return CheckObstacles(cellCoordinates);
    }
    private bool CheckObstacles(Vector3Int cellCoordinates)
    {
        Vector2Int cellCoords = new Vector2Int(cellCoordinates.x, cellCoordinates.y);
        foreach (Obstacle obstacle in obstacles)
        {
            if (obstacle.cellCoordinates == cellCoords)
                return true;
        }
        return false;
    }
    public void ArrivateAtCell(Vector3Int cellCoordinates)
    {
        CheckInteractables(cellCoordinates);
    }
    private void CheckInteractables(Vector3Int cellCoordinates)
    {
        foreach (Interactable interactable in interactables)
        {
            if (interactable.IsInInteractRange(cellCoordinates))
                interactable.ShowInteractPanel();
        }
    }
}
