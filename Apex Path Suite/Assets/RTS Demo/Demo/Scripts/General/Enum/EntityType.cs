namespace Apex.Demo.RTS.AI
{
    /// <summary>
    /// Enum type for the different kinds of entities in the RTS demo. 
    /// None and 'Any' values are useful for generic AI elements.
    /// </summary>
    public enum EntityType
    {
        None = 0,
        Any = ~0,
        Unit = 1,
        Structure,
        Resource
    }
}