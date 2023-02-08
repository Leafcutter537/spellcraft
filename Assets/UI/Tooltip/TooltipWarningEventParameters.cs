using Assets.EventSystem;

public class TooltipWarningEventParameters : EventParameters
{
    public string warningMessage;

    public TooltipWarningEventParameters(string warningMessage)
    {
        this.warningMessage = warningMessage;
    }
}
