namespace Apex.Demo.RTS.AI
{
    using UnityEngine;

    /// <summary>
    /// Represents the pool for units.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.PoolBase{Apex.Demo.RTS.AI.IUnit}" />
    public sealed class UnitPool : PoolBase<IUnit>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitPool"/> class.
        /// </summary>
        /// <param name="prefab">The prefab from which to create the entity.</param>
        /// <param name="host">The host that will be the parent of entity instances.</param>
        /// <param name="initialInstanceCount">The initial instance count.</param>
        public UnitPool(GameObject prefab, GameObject host, int initialInstanceCount)
            : base(prefab, host, initialInstanceCount)
        {
        }
    }
}