namespace Apex.Demo.RTS.AI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Component for handling particle systems and their pools. This component is a singleton and will ensure that it is the only one in the scene.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.SingletonMonoBehaviour{Apex.Demo.RTS.AI.ParticlePoolManager}" />
    public class ParticlePoolManager : SingletonMonoBehaviour<ParticlePoolManager>
    {
        private static readonly int particleTypeLength = Enum.GetNames(typeof(ParticlesType)).Length;

        [SerializeField]
        private ParticlePoolSetup[] _poolSetup = new ParticlePoolSetup[particleTypeLength];

        private readonly Dictionary<ParticlesType, ParticlePool> _pools = new Dictionary<ParticlesType, ParticlePool>(particleTypeLength, new ParticlesTypeComparer());

        protected override void Awake()
        {
            base.Awake();

            // Setup all pools
            // Note: can be reduced to one-level transform hierarchy for performance
            var managerHost = new GameObject("Particles");
            managerHost.transform.SetParent(this.transform);

            for (int i = 0; i < _poolSetup.Length; i++)
            {
                var setup = _poolSetup[i];

                var host = new GameObject(setup.type.ToString());
                host.transform.SetParent(managerHost.transform);

                _pools.Add(setup.type, new ParticlePool(setup.prefab, host, setup.initialInstanceCount));
            }
        }

        /// <summary>
        /// Spawns a particle system of the given type at the specified position, with default rotation.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="position">The position.</param>
        public void SpawnParticleSystem(ParticlesType type, Vector3 position)
        {
            SpawnParticleSystem(type, position, Quaternion.identity);
        }

        /// <summary>
        /// Spawns a particle system of the given type at the specified position, with the desired rotation.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="position">The position.</param>
        /// <param name="rotation">The rotation.</param>
        public void SpawnParticleSystem(ParticlesType type, Vector3 position, Quaternion rotation)
        {
            var particles = _pools[type].Get(position, rotation);
            particles.Play();

            if (this.gameObject.activeSelf)
            {
                // only return systems if this manager is still active, otherwise the game is shutting down anyway
                StartCoroutine(ReturnSystem(type, particles));
            }
        }

        /// <summary>
        /// Returns the given particle system to the pool from whence it came.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="system">The system.</param>
        /// <returns></returns>
        private IEnumerator ReturnSystem(ParticlesType type, IParticleSystem system)
        {
            yield return new WaitForSeconds(system.duration);
            _pools[type].Return(system);
        }
    }
}