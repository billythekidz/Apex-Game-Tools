namespace Apex.Demo.RTS.AI
{
    /// <summary>
    /// Interface representing particle system components that are instance pooled.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.IPooled" />
    public interface IParticleSystem : IPooled
    {
        /// <summary>
        /// Gets the duration of the particle system, as set on the particlce system.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        float duration { get; }

        /// <summary>
        /// Play the particle system.
        /// </summary>
        void Play();
    }
}