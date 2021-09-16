namespace Apex.Demo.RTS.AI
{
    using UnityEngine;

    /// <summary>
    /// Instance pool for all decals.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.PoolBase{Apex.Demo.RTS.AI.IDecal}" />
    public sealed class DecalPool : PoolBase<IDecal>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DecalPool"/> class.
        /// </summary>
        /// <param name="prefab">The prefab from which to create the entity.</param>
        /// <param name="host">The host that will be the parent of entity instances.</param>
        /// <param name="initialInstanceCount">The initial instance count.</param>
        public DecalPool(GameObject prefab, GameObject host, int initialInstanceCount)
            : base(prefab, host, initialInstanceCount)
        {
        }
    }
}