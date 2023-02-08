using Assets.EventSystem;

public class CombatLogEventParameters : EventParameters
{
    public string messageString;

    public CombatLogEventParameters(string messageString)
    {
        this.messageString = messageString;
    }
}
