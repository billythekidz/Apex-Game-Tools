namespace Apex.Demo.RTS.AI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Manages the structure pool so that spawning and returning structures become as convenient and easy as possible for the rest of the codebase.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.SingletonMonoBehaviour{Apex.Demo.RTS.AI.StructurePoolManager}" />
    public sealed class StructurePoolManager : SingletonMonoBehaviour<StructurePoolManager>
    {
        private static readonly int structureTypeLength = Enum.GetNames(typeof(StructureType)).Length - 2;

        [SerializeField]
        private StructurePoolSetup[] _poolSetup = new StructurePoolSetup[structureTypeLength];

        private readonly Dictionary<StructureType, StructurePool> _pools = new Dictionary<StructureType, StructurePool>(structureTypeLength, new StructureTypeComparer());

        /// <summary>
        /// Called by Unity when this instance awakes.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            // Setup the pools and initialize them
            // Note: Can be reduced to one-level transform hierarchy for performance
            var managerHost = new GameObject("Structures");
            managerHost.transform.SetParent(this.transform);

            for (int i = 0; i < _poolSetup.Length; i++)
            {
                var setup = _poolSetup[i];

                var host = new GameObject(setup.type.ToString());
                host.transform.SetParent(managerHost.transform);

                _pools.Add(setup.type, new StructurePool(setup.prefab, host, setup.initialInstanceCount));
            }
        }

        /// <summary>
        /// Builds a structure of the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public bool BuildStructure(StructureType type, WorkerUnit builder)
        {
            // only valid types
            if (type == StructureType.None || type == StructureType.Any || !_pools.ContainsKey(type))
            {
                Debug.LogError(this.ToString() + " cannot spawn structures of type (not supported): " + type);
                return false;
            }

            var controller = builder.controller;

            // cost/resource
            var costs = CostHelper.GetCost(type);
            if (!controller.HasResources(costs))
            {
                // cannot afford
                return false;
            }

            // spend resources
            controller.SpendResources(costs);

            InternalBuild(type, controller, builder.position, costs);
            return true;
        }

        /// <summary>
        /// Gets the Center structure that AI controllers start with. Checks if they already had a Center structure assigned through the Unity inspector, otherwise builds a new one instantly.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <returns></returns>
        public Center GetStartCenter(AIController controller)
        {
            return controller.center ?? (Center)InternalBuild(StructureType.Center, controller, controller.position, new ResourceCost(0f));
        }

        /// <summary>
        /// Internal function that actually executes the construction of a structure.
        /// </summary>
        /// <param name="type">The type of structure to build.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="position">The position.</param>
        /// <param name="costs">The construction costs.</param>
        /// <returns></returns>
        private IStructure InternalBuild(StructureType type, AIController controller, Vector3 position, ResourceCost costs)
        {
            // instantiate structure
            var pool = _pools[type];

            // fix structures to the ground
            var pos = Utils.GetGroundedPosition(position);
            var structure = pool.Get(pos, Quaternion.identity);
            structure.controller = controller;

            // manage structure ready state - structure is ready if the cost time is zero
            structure.isReady = costs.time == 0f;
            if (!structure.isReady)
            {
                // if structure is not ready, then delay ready setting by cost time
                StartCoroutine(SetStructureReady(structure, costs.time));
            }

            // color structure
            structure.gameObject.ColorRenderers(controller.color);

            // add to controller's list
            controller.structures.Add(structure);

            // occupy a spot in the controller's structure grid
            var nearest = controller.structureGrid.GetNearestCell(pos);
            nearest.occupied = true;

            return structure;
        }

        /// <summary>
        /// Sets the structure ready after the specified delay.
        /// </summary>
        /// <param name="structure">The structure.</param>
        /// <param name="delay">The delay.</param>
        /// <returns></returns>
        private IEnumerator SetStructureReady(IStructure structure, float delay)
        {
            yield return new WaitForSeconds(delay);
            structure.isReady = true;
        }

        /// <summary>
        /// Returns the specified structure to the pool from whence it came.
        /// </summary>
        /// <param name="structure">The structure.</param>
        public void Return(IStructure structure)
        {
            _pools[structure.structureType].Return(structure);
        }
    }
}