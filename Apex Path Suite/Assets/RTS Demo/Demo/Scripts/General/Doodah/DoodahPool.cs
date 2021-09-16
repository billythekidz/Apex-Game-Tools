namespace Apex.Demo.RTS.AI
{
    using UnityEngine;

    /// <summary>
    /// Instance pool for doodahs.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.PoolBase{Apex.Demo.RTS.AI.IDoodah}" />
    public sealed class DoodahPool : PoolBase<IDoodah>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DoodahPool"/> class.
        /// </summary>
        /// <param name="prefab">The prefab from which to create the entity.</param>
        /// <param name="host">The host that will be the parent of entity instances.</param>
        /// <param name="initialInstanceCount">The initial instance count.</param>
        public DoodahPool(GameObject prefab, GameObject host, int initialInstanceCount)
            : base(prefab, host, initialInstanceCount)
        {
        }
    }
}