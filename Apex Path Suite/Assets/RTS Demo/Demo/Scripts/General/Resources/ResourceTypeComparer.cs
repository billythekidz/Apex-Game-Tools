namespace Apex.Demo.RTS.AI
{
    using System.Collections.Generic;

    /// <summary>
    /// Custom type comparer for comparing resource types without memory allocations due to boxing.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEqualityComparer{Apex.Demo.RTS.AI.ResourceType}" />
    public sealed class ResourceTypeComparer : IEqualityComparer<ResourceType>
    {
        public bool Equals(ResourceType x, ResourceType y)
        {
            return x == y;
        }

        public int GetHashCode(ResourceType obj)
        {
            return (int)obj;
        }
    }
}