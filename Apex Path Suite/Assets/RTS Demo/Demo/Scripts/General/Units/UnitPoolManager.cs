namespace Apex.Demo.RTS.AI
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Manager for unit pools. Singleton in the scene which manages pool initialization, spawning units from the pool and returning them.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.SingletonMonoBehaviour{Apex.Demo.RTS.AI.UnitPoolManager}" />
    public sealed class UnitPoolManager : SingletonMonoBehaviour<UnitPoolManager>
    {
        private static readonly int unitTypeLength = Enum.GetNames(typeof(UnitType)).Length - 2;

        [SerializeField]
        private UnitPoolSetup[] _poolSetup = new UnitPoolSetup[unitTypeLength];

        private readonly Dictionary<UnitType, UnitPool> _pools = new Dictionary<UnitType, UnitPool>(unitTypeLength, new UnitTypeComparer());

        /// <summary>
        /// Called by Unity when this instance awakes.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            //note -> can be reduced to one-level transform hierarchy for performance
            var managerHost = new GameObject("Units");
            managerHost.transform.SetParent(this.transform);

            for (int i = 0; i < _poolSetup.Length; i++)
            {
                var setup = _poolSetup[i];

                var host = new GameObject(setup.type.ToString());
                host.transform.SetParent(managerHost.transform);

                _pools.Add(setup.type, new UnitPool(setup.prefab, host, setup.initialInstanceCount));
            }
        }

        /// <summary>
        /// Spawns a unit of the specified type, with the given parameters.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="position">The position.</param>
        /// <param name="spawnDistance">The spawn distance.</param>
        /// <param name="anglePerSpawn">The angle per spawn.</param>
        /// <param name="spawnIndex">Index of the spawn.</param>
        /// <returns></returns>
        public bool SpawnUnit(UnitType type, AIController controller, Vector3 position, float spawnDistance, float anglePerSpawn, int spawnIndex)
        {
            // only valid types
            if (type == UnitType.None || type == UnitType.Any || !_pools.ContainsKey(type))
            {
                Debug.LogError(this.ToString() + " cannot spawn units of type (not supported): " + type);
                return false;
            }

            InternalSpawnUnit(type, controller, position, spawnDistance, anglePerSpawn, spawnIndex);
            return true;
        }

        /// <summary>
        /// Spawns the given count of worker units with the specified parameters. Used for spawning the starting workers.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="count">The count.</param>
        /// <param name="spawnDistance">The spawn distance.</param>
        /// <param name="anglePerSpawn">The angle per spawn.</param>
        /// <param name="spawnIndex">Index of the spawn.</param>
        public void GetStartWorkers(AIController controller, int count, float spawnDistance, float anglePerSpawn, int spawnIndex)
        {
            for (int i = 0; i < count; i++)
            {
                InternalSpawnUnit(UnitType.Worker, controller, controller.position, spawnDistance, anglePerSpawn, spawnIndex++);
            }
        }

        /// <summary>
        /// The internal unit spawning method, which actually spawns units from the unit pool.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="position">The position.</param>
        /// <param name="spawnDistance">The spawn distance.</param>
        /// <param name="anglePerSpawn">The angle per spawn.</param>
        /// <param name="spawnIndex">Index of the spawn.</param>
        /// <returns></returns>
        private IUnit InternalSpawnUnit(UnitType type, AIController controller, Vector3 position, float spawnDistance, float anglePerSpawn, int spawnIndex)
        {
            // spawn unit
            var pool = _pools[type];
            var pos = CircleHelpers.GetPointOnCircle(position, spawnDistance, anglePerSpawn, spawnIndex);

            pos = Utils.GetGroundedPosition(pos) + new Vector3(0f, 0.5f, 0f); // TODO: tried placing units a bit above ground to avoid weird NavMesh bug with sliding
            var unit = pool.Get(pos, Quaternion.identity);

            // color unit
            unit.gameObject.ColorRenderers(controller.color);

            // add to controller's unit list
            unit.controller = controller;
            controller.units.Add(unit);
            controller.defaultGroup.Add(unit);

            return unit;
        }

        /// <summary>
        /// Returns the specified unit to the unit pool from whence it came.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public void Return(IUnit unit)
        {
            _pools[unit.unitType].Return(unit);
        }
    }
}