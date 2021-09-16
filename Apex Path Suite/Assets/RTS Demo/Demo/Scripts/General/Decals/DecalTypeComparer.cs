namespace Apex.Demo.RTS.AI
{
    using System.Collections.Generic;

    /// <summary>
    /// Custom comparer for comparing DecalType enums without memory allocations due to boxing.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEqualityComparer{Apex.Demo.RTS.AI.DecalType}" />
    public sealed class DecalTypeComparer : IEqualityComparer<DecalType>
    {
        public bool Equals(DecalType x, DecalType y)
        {
            return x == y;
        }

        public int GetHashCode(DecalType obj)
        {
            return (int)obj;
        }
    }
}