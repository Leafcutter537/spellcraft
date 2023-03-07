using Assets.EventSystem;

public class WarningInteractableEventParameters : EventParameters
{
    public string message;
    public WarningInteractableEventParameters(string message)
    {
        this.message = message;
    }
}
