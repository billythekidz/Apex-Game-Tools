namespace Apex.Demo.RTS.AI
{
    /// <summary>
    /// Interface representing decals.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.IPooled" />
    public interface IDecal : IPooled
    {
        /// <summary>
        /// Gets or sets the type of the decal.
        /// </summary>
        /// <value>
        /// The type of the decal.
        /// </value>
        DecalType decalType { get; set; }
    }
}