namespace Apex.Demo.RTS.AI
{
    /// <summary>
    /// Enum type for the different kinds of decals in the RTS demo.
    /// None and 'Any' values are useful for generic elements.
    /// </summary>
    public enum DecalType
    {
        None = 0,
        Any = ~0,
        ExplosionSmall = 1,
        ExplostionLarge,
    }
}