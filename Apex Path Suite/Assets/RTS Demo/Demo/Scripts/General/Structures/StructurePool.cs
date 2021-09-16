namespace Apex.Demo.RTS.AI
{
    using UnityEngine;

    /// <summary>
    /// Represents a structure pool.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.PoolBase{Apex.Demo.RTS.AI.IStructure}" />
    public sealed class StructurePool : PoolBase<IStructure>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StructurePool"/> class.
        /// </summary>
        /// <param name="prefab">The prefab from which to create the entity.</param>
        /// <param name="host">The host that will be the parent of entity instances.</param>
        /// <param name="initialInstanceCount">The initial instance count.</param>
        public StructurePool(GameObject prefab, GameObject host, int initialInstanceCount)
            : base(prefab, host, initialInstanceCount)
        {
        }
    }
}