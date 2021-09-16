namespace Apex.Demo.RTS.AI
{
    /// <summary>
    /// Enum type for the different kinds of units in the RTS demo.
    /// None and 'Any' values are useful for generic AI elements.
    /// </summary>
    public enum UnitType
    {
        None = 0,
        Any = ~0,
        Melee = 1,
        Ranged,
        Siege,
        Worker,
    }
}