namespace Apex.Demo.RTS.AI
{
    /// <summary>
    /// Enum type for the different kinds of resources in the RTS demo.
    /// None and 'Any' values are useful for generic AI elements.
    /// </summary>
    public enum ResourceType
    {
        None = 0,
        Any = ~0,
        Metal = 1,
        Energy,
    }
}