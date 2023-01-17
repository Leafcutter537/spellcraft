using Assets.EventSystem;
using Assets.Inventory.Runes;

public class RuneEventParameters : EventParameters
{
    public Rune rune;
    public RuneEventParameters(Rune rune)
    {
        this.rune = rune;
    }

}
