namespace Apex.Demo.RTS.AI
{
    /// <summary>
    /// Enum type for the different kinds of structures in the RTS demo.
    /// None and 'Any' values are useful for generic AI elements.
    /// </summary>
    public enum StructureType
    {
        None = 0,
        Any = ~0,
        Center = 1,
        Factory,
        Cannon,
        Radar,
        EnergyGenerator
    }
}