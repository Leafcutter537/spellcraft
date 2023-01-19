using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private WorldMap worldMap;
    public Vector2Int cellCoordinates;

    private void Start()
    {
        worldMap.SubscribeObstacle(this);
    }
}