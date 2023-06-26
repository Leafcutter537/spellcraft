using Assets.EventSystem;
using Assets.Combat;

public class AdjacentPreviewEventParameters : EventParameters
{
    public GridSquare square;
    public string entityName;

    public AdjacentPreviewEventParameters(GridSquare square, string entityName)
    {
        this.square = square;
        this.entityName = entityName;
    }
}
