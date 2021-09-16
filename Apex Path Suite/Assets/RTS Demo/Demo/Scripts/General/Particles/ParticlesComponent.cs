namespace Apex.Demo.RTS.AI
{
    using UnityEngine;

    /// <summary>
    /// Component for particle systems. This component must be present on pooled particle systems.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.PooledBase" />
    /// <seealso cref="Apex.Demo.RTS.AI.IParticleSystem" />
    [RequireComponent(typeof(ParticleSystem))]
    public sealed class ParticlesComponent : PooledBase, IParticleSystem
    {
        private ParticleSystem _system;

        /// <summary>
        /// Gets the duration of the particle system, as set on the particlce system.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        public float duration
        {
            get { return _system.duration; }
        }

        /// <summary>
        /// Called by Unity when [enabled].
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();

            _system = this.GetComponent<ParticleSystem>();
        }

        /// <summary>
        /// Play the particle system.
        /// </summary>
        public void Play()
        {
            _system.Play();
        }
    }
}