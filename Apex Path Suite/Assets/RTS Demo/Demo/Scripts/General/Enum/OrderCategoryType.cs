namespace Apex.Demo.RTS.AI
{
    /// <summary>
    /// Enum type representing different categories of orders.
    /// </summary>
    public enum OrderCategoryType
    {
        None = 0,
        Any = ~0,
        Defensive = 1,
        Offensive,
        Construction,
        Scouting,
    }
}