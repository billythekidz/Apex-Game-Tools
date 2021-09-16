namespace Apex.Demo.RTS.AI
{
    using System.Collections.Generic;

    /// <summary>
    /// A type comparer for comparing structure types without memory allocations due to boxing.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEqualityComparer{Apex.Demo.RTS.AI.StructureType}" />
    public sealed class StructureTypeComparer : IEqualityComparer<StructureType>
    {
        public bool Equals(StructureType x, StructureType y)
        {
            return x == y;
        }

        public int GetHashCode(StructureType obj)
        {
            return (int)obj;
        }
    }
}