using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [Header("References to World")]
    [SerializeField] private Tilemap worldTilemap;
    [SerializeField] private WorldMap worldMap;
    [Header("Movement")]
    [SerializeField] private float timeToMove;
    private float timeSinceMoveInitiated;
    private bool isMoving;
    public Vector2Int currentLocation;
    private Vector2Int targetLocation;
    [SerializeField] private bool isInDungeon;
    [Header("Event References")]
    [SerializeField] private LeaveCellEvent leaveCellEvent;
    [Header("Serialized Object References")]
    [SerializeField] private ProgressTracker progressTracker;

    private void Start()
    {
        timeSinceMoveInitiated = timeToMove;
        if (!isInDungeon)
            SetLocation(progressTracker.playerPosition);
    }

    private void Update()
    {
        if (isMoving)
        {
            if (timeSinceMoveInitiated < timeToMove)
            {
                Vector2 currentLocationWorld = worldTilemap.CellToWorld((Vector3Int) currentLocation);
                Vector2 targetLocationWorld = worldTilemap.CellToWorld((Vector3Int) targetLocation);
                Vector2 diffVector = targetLocationWorld - currentLocationWorld;
                transform.position = currentLocationWorld + diffVector * (timeSinceMoveInitiated) / timeToMove;
                timeSinceMoveInitiated += Time.deltaTime;
            }
            else
            {
                currentLocation = targetLocation;
                worldMap.ArrivateAtCell(targetLocation);
                isMoving = false;
            }
        }
    }
    public void MoveUp()
    {
        if (!isMoving) { MoveToDisplacement(new Vector2Int(0, 1)); }
    }
    public void MoveDown()
    {
        if (!isMoving) { MoveToDisplacement(new Vector2Int(0, -1)); }
    }
    public void MoveLeft()
    {
        if (!isMoving) { MoveToDisplacement(new Vector2Int(-1, 0)); }
    }
    public void MoveRight()
    {
        if (!isMoving) { MoveToDisplacement(new Vector2Int(1, 0)); }
    }
    private void MoveToDisplacement(Vector2Int displacementVector)
    {
        if (!worldMap.IsCellBlocked(currentLocation + displacementVector))
        {
            targetLocation = currentLocation + displacementVector;
            timeSinceMoveInitiated = 0;
            isMoving = true;
            leaveCellEvent.Raise(this, null);
            if (!isInDungeon)
                progressTracker.playerPosition = targetLocation;
        }
    }

    public void SetLocation(Vector2Int newLocation)
    {
        transform.position = new Vector3(newLocation.x, newLocation.y);
        currentLocation = newLocation;
    }
}

