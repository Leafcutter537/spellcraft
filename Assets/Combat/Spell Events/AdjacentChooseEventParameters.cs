using Assets.Combat;
using Assets.EventSystem;
using UnityEngine;

public class AdjacentChooseEventParameters : EventParameters
{
    public Vector2Int adjacentVector;
    public GridSquare gridSquare;

    public AdjacentChooseEventParameters(Vector2Int adjacentVector, GridSquare gridSquare)
    {
        this.adjacentVector = adjacentVector;
        this.gridSquare = gridSquare;
    }
}
