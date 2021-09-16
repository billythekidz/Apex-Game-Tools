namespace Apex.Demo.RTS.AI
{
    using System.Collections.Generic;

    /// <summary>
    /// Custom comparer for particle type enum to avoid memory allocations.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEqualityComparer{Apex.Demo.RTS.AI.ParticlesType}" />
    public sealed class ParticlesTypeComparer : IEqualityComparer<ParticlesType>
    {
        public bool Equals(ParticlesType x, ParticlesType y)
        {
            return x == y;
        }

        public int GetHashCode(ParticlesType obj)
        {
            return (int)obj;
        }
    }
}