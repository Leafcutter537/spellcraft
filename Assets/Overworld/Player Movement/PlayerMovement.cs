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
    public Vector3Int currentLocation;
    private Vector3Int targetLocation;
    [Header("Event References")]
    [SerializeField] private LeaveCellEvent leaveCellEvent;

    private void Awake()
    {
        timeSinceMoveInitiated = timeToMove;
        currentLocation = new Vector3Int(0, 0, 0);
    }

    private void Update()
    {
        if (isMoving)
        {
            if (timeSinceMoveInitiated < timeToMove)
            {
                Vector2 currentLocationWorld = worldTilemap.CellToWorld(currentLocation);
                Vector2 targetLocationWorld = worldTilemap.CellToWorld(targetLocation);
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
        if (!isMoving) { MoveToDisplacement(new Vector3Int(0, 1)); }
    }
    public void MoveDown()
    {
        if (!isMoving) { MoveToDisplacement(new Vector3Int(0, -1)); }
    }
    public void MoveLeft()
    {
        if (!isMoving) { MoveToDisplacement(new Vector3Int(-1, 0)); }
    }
    public void MoveRight()
    {
        if (!isMoving) { MoveToDisplacement(new Vector3Int(1, 0)); }
    }
    private void MoveToDisplacement(Vector3Int displacementVector)
    {
        if (!worldMap.IsCellBlocked(currentLocation + displacementVector))
        {
            targetLocation = currentLocation + displacementVector;
            timeSinceMoveInitiated = 0;
            isMoving = true;
            leaveCellEvent.Raise(this, null);
        }
    }
}

