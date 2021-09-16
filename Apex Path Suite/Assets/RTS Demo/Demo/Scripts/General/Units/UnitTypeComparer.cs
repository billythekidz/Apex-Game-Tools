namespace Apex.Demo.RTS.AI
{
    using System.Collections.Generic;

    /// <summary>
    /// Custom type comparer for comparing unit types.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEqualityComparer{Apex.Demo.RTS.AI.UnitType}" />
    public class UnitTypeComparer : IEqualityComparer<UnitType>
    {
        public bool Equals(UnitType x, UnitType y)
        {
            return x == y;
        }

        public int GetHashCode(UnitType obj)
        {
            return (int)obj;
        }
    }
}