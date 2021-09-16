namespace Apex.Demo.RTS.AI
{
    /// <summary>
    /// Enum type for the different kinds of orders that controllers can give to units and groups.
    /// None and 'Any' values are useful for generic AI elements.
    /// </summary>
    public enum OrderType
    {
        None = 0,
        Any = ~0,

        // individual order types
        BuildStructure = 1,
        Scout,

        // group order types
        FrontalAttack = 10,
        WeakestPointAttack,
        FlankingAttack,
        DirectBaseAttack,
        CounterAttack,
        RetreatToBase,
    }
}