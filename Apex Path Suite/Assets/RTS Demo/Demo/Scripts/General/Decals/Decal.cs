namespace Apex.Demo.RTS.AI
{
    /// <summary>
    /// Component representing decals, which are visual effects on the ground as a result of explosions or similar.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.PooledBase" />
    /// <seealso cref="Apex.Demo.RTS.AI.IDecal" />
    public sealed class Decal : PooledBase, IDecal
    {
        /// <summary>
        /// Gets or sets the type of the decal.
        /// </summary>
        /// <value>
        /// The type of the decal.
        /// </value>
        public DecalType decalType { get; set; }
    }
}