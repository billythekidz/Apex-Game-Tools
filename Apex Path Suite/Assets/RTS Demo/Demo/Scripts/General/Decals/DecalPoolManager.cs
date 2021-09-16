namespace Apex.Demo.RTS.AI
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Component for handling decals and their pools. This component is a singleton and will ensure that it is the only one in the scene.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.SingletonMonoBehaviour{Apex.Demo.RTS.AI.DecalPoolManager}" />
    public sealed class DecalPoolManager : SingletonMonoBehaviour<DecalPoolManager>
    {
        private static readonly int decalTypeLength = Enum.GetNames(typeof(DecalType)).Length - 2;

        [SerializeField, Range(0.001f, 1f), Tooltip("The decal's offset from the ground. To avoid Z-fighting decals are raised by this value above ground.")]
        private float _decalGroundOffset = 0.05f;

        [SerializeField]
        private DecalPoolSetup[] _poolSetup = new DecalPoolSetup[decalTypeLength];

        private readonly Dictionary<DecalType, DecalPool> _pools = new Dictionary<DecalType, DecalPool>(decalTypeLength, new DecalTypeComparer());

        protected override void Awake()
        {
            base.Awake();

            // Setup the pools and initialize them
            // Note: Can be reduced to one-level transform hierarchy for performance
            var managerHost = new GameObject("Decals");
            managerHost.transform.SetParent(this.transform);

            for (int i = 0; i < _poolSetup.Length; i++)
            {
                var setup = _poolSetup[i];

                var host = new GameObject(setup.type.ToString());
                host.transform.SetParent(managerHost.transform);

                _pools.Add(setup.type, new DecalPool(setup.prefab, host, setup.initialInstanceCount));
            }
        }

        /// <summary>
        /// Spawns a decal of the given type at the specified position.
        /// </summary>
        /// <param name="decalType">Type of the decal.</param>
        /// <param name="position">The position.</param>
        public void SpawnDecal(DecalType decalType, Vector3 position)
        {
            // spawn decal
            var pool = _pools[decalType];

            //we need to check if the position is walkable
            var pos = Utils.GetGroundedPosition(position);

            //offset the decal a little vertically to not blur with the ground texture
            pool.Get(pos + (Vector3.up * _decalGroundOffset), Quaternion.LookRotation(Vector3.down));
        }

        /// <summary>
        /// Returns the specified decal to the pool from whence it came.
        /// </summary>
        /// <param name="decal">The decal.</param>
        public void Return(IDecal decal)
        {
            _pools[decal.decalType].Return(decal);
        }
    }
}