namespace Apex.Demo.RTS.AI
{
    using System;
    using System.Collections.Generic;
    using Apex.AI;
    using Apex.AI.Components;
    using UnityEngine;

    /// <summary>
    /// Component representing the controller of the AI, responsible for overall strategic and tactical decisions, including resource management, building structures, spawning units and issuing orders.
    /// </summary>
    public sealed class AIController : MonoBehaviour, IHasMemory, IContextProvider
    {
        [SerializeField, Tooltip("The color for the AI. All structures and units will be tinted by this color.")]
        private Color _color = Color.red;

        [SerializeField, Range(0.1f, 60f), Tooltip("How long the AI considers itself 'under attack' after an attack on a structure occured.")]
        private float _underAttackTimeThreshold = 10f;

        [Header("Start")]
        [SerializeField, Tooltip("Use this field to manually assign a Center structure to the AI, if so desired. If none is assigned, it will be created automatically on start.")]
        private Center _center;

        [SerializeField, Range(0, 10), Tooltip("How many workers this AI starts with.")]
        private int _startWorkers = 5;

        [SerializeField, Range(0, 10000), Tooltip("How much energy resources this AI starts with.")]
        private int _startEnergy = 500;

        [SerializeField, Range(0, 10000), Tooltip("How much metal resources this AI starts with.")]
        private int _startMetal = 1000;

        [SerializeField, Tooltip("The starting strategy for this AI. Can be assigned manually, if not the AI will decide randomly on a strategy.")]
        private StrategyType _startStrategy = StrategyType.None;

        [Header("Structure Grid")]
        [SerializeField, Range(10f, 100f), Tooltip("How large the structure grid is, with the Center structure positioned in the center of the grid.")]
        private float _structureGridSize = 40f;

        [SerializeField, Range(1f, 20f), Tooltip("How much spacing there is between each structure grid cell.")]
        private float _structureGridSpacing = 5f;

        [SerializeField, Range(0.5f, 10f), Tooltip("How large each structure grid cell is.")]
        private float _structureGridCellSize = 2f;

        [Header("Map Grid")]
        [SerializeField, Range(0.01f, 2f), Tooltip("How much spacing there is between map grid cells.")]
        private float _mapGridSpacing = 0.1f;

        [SerializeField, Range(0.5f, 20f), Tooltip("How large each map grid cell is.")]
        private float _mapGridCellSize = 5f;

        // prepare a dictionary for the resources automatically preallocated at the right size, accounting for new resource types added to the enum ResourceType
        // Also use a custom comparer to avoid memory allocations (boxing) when comparing enums
        private readonly Dictionary<ResourceType, int> _currentResources = new Dictionary<ResourceType, int>(Enum.GetNames(typeof(ResourceType)).Length - 2, new ResourceTypeComparer());

        private ControllerContext _context;

        /// <summary>
        /// The color for the AI. All structures and units will be tinted by this color.
        /// </summary>
        public Color color
        {
            get { return _color; }
        }

        /// <summary>
        /// The Center structure for this
        /// </summary>
        public Center center
        {
            get { return _center; }
        }

        /// <summary>
        /// The position of the AI is actually just the position of the Center structure.
        /// </summary>
        public Vector3 position
        {
            get { return _center != null ? _center.position : this.transform.position; }
        }

        /// <summary>
        /// All the AI's units.
        /// </summary>
        public IList<IUnit> units
        {
            get;
            private set;
        }

        /// <summary>
        /// All the AI's unit groups.
        /// </summary>
        public IList<UnitGroup> groups
        {
            get;
            private set;
        }

        /// <summary>
        /// The default group for the AI is always the group at the 0th index. This group should never be removed, as it is used for newly spawned units.
        /// </summary>
        public UnitGroup defaultGroup
        {
            get { return this.groups != null && this.groups.Count > 0 ? this.groups[0] : null; }
        }

        /// <summary>
        /// All the AI's structures.
        /// </summary>
        public IList<IStructure> structures
        {
            get;
            private set;
        }

        /// <summary>
        /// All the AI's observations. Includes observations of resources, own units and structures as well as enemies.
        /// </summary>
        public IList<Observation> observations
        {
            get;
            private set;
        }

        /// <summary>
        /// All the AI's observations of enemies. Includes enemy units and structures.
        /// </summary>
        public IList<Observation> enemyObservations
        {
            get;
            private set;
        }

        /// <summary>
        /// The structure grid for this AI, used for the construction of structures. Structures are built on structure grid cells.
        /// </summary>
        public StructureGrid structureGrid
        {
            get;
            private set;
        }

        /// <summary>
        /// The map grid for this AI. The map grid spans the entire map and is used to evaluate which areas of the map have been seen, in addition to being used as an influence map for evaluating threat.
        /// </summary>
        public MapGrid mapGrid
        {
            get;
            private set;
        }

        /// <summary>
        /// The currently employed strategy for this AI.
        /// </summary>
        public StrategyType strategy
        {
            get;
            set;
        }

        /// <summary>
        /// Whether this AI has very recently had a structure being attacked by enemies.
        /// </summary>
        public bool isUnderAttack
        {
            get { return Time.timeSinceLevelLoad - this.lastAttack < _underAttackTimeThreshold; }
        }

        /// <summary>
        /// The last time one of this AI's structures was attacked, in seconds since level load.
        /// </summary>
        public float lastAttack
        {
            get;
            set;
        }

        /// <summary>
        /// The predicted position of the enemy base. This is currently extremely simple, in that the AI expects its opponent to be on the exact opposite side of the center of the map.
        /// </summary>
        public Vector3 predictedEnemyBasePosition
        {
            get;
            set;
        }

        /// <summary>
        /// Unity's OnEnable, called whenever this component is enabled.
        /// </summary>
        private void OnEnable()
        {
            // Prepare lists, grids and more.
            this.units = new List<IUnit>(25); // TODO: Consider pre-allocation values
            this.groups = new List<UnitGroup>(4);
            this.groups.Add(new UnitGroup(10));
            this.structures = new List<IStructure>(5);
            this.observations = new List<Observation>(100);
            this.enemyObservations = new List<Observation>(50);
            this.structureGrid = new StructureGrid(this.position, _structureGridSize, _structureGridSpacing, _structureGridCellSize);
            this.mapGrid = new MapGrid(Vector3.zero, Utils.GetTotalMapSize() - _mapGridCellSize, _mapGridSpacing, _mapGridCellSize);
            this.strategy = _startStrategy;
            this.lastAttack = float.MinValue;

            // Prepare the AI's context object
            _context = new ControllerContext(this);
            _center = StructurePoolManager.instance.GetStartCenter(this); // constructs a center if one has not been set manually in Unity inspector
            _center.GetStartWorkers(_startWorkers);

            // add starting resources
            AddResource(ResourceType.Energy, _startEnergy);
            AddResource(ResourceType.Metal, _startMetal);

            // predict enemy position (TODO: Maybe move to AI ?)
            this.predictedEnemyBasePosition = -this.position;
        }

        /// <summary>
        /// Unity's OnDisable, called whenver this component is disabled.
        /// </summary>
        private void OnDisable()
        {
            // null all lists and grids to aid proper garbage collection.
            _context = null;
            _center = null;
            this.strategy = StrategyType.None;
            this.mapGrid = null;
            this.structureGrid = null;
            this.enemyObservations = null;
            this.observations = null;
            this.structures = null;
            this.groups = null;
            this.units = null;
        }

        /// <summary>
        /// Called when the Controller has lost the game, due to the destruction of its Center structure.
        /// </summary>
        public void OnDeath()
        {
            // destroy all units and structures when the AI controller is disabled
            var count = this.units.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                UnitPoolManager.instance.Return(this.units[i]);
            }

            count = this.structures.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                StructurePoolManager.instance.Return(this.structures[i]);
            }

            this.gameObject.SetActive(false);
        }

        /// <summary>
        /// The current amount of units for this AI, of the given type.
        /// </summary>
        /// <param name="unitType"></param>
        /// <returns>The count of units of the given type.</returns>
        public int GetUnitCount(UnitType unitType)
        {
            if (unitType == UnitType.None)
            {
                // If the supplied type is 'None', always return 0
                return 0;
            }

            var count = this.units.Count;
            if (count == 0)
            {
                return 0;
            }

            if (unitType == UnitType.Any)
            {
                // If the supplied type is 'Any', just return the count, no need for iterating.
                return count;
            }

            var counted = 0;
            for (int i = 0; i < count; i++)
            {
                var unit = this.units[i];
                if (unit.unitType != unitType)
                {
                    continue;
                }

                counted++;
            }

            return counted;
        }

        /// <summary>
        /// The current amount of structures for this AI, of the given type.
        /// </summary>
        /// <param name="structureType"></param>
        /// <returns>The count of structures of the given type.</returns>
        public int GetStructureCount(StructureType structureType)
        {
            if (structureType == StructureType.None)
            {
                // If the supplied type is 'None', always return 0
                return 0;
            }

            var count = this.structures.Count;
            if (count == 0)
            {
                return 0;
            }

            if (structureType == StructureType.Any)
            {
                // If the supplied type is 'Any', just return the count, no need for iterating.
                return count;
            }

            var counted = 0;
            for (int i = 0; i < count; i++)
            {
                var structure = this.structures[i];
                if (structure.structureType != structureType)
                {
                    continue;
                }

                counted++;
            }

            return counted;
        }

        /// <summary>
        /// The amount of structures of the given type currently in orders issued by this AI.
        /// </summary>
        /// <param name="structureType"></param>
        /// <returns>The count of structures of the given type currently in units' orders.</returns>
        public int GetStructureCountInOrders(StructureType structureType)
        {
            if (structureType == StructureType.None)
            {
                // If the supplied type is 'None', always return 0
                return 0;
            }

            // First check units' currently executing order, then the current order and finally iterate through all of the units' non-completed orders
            var counted = 0;
            var count = units.Count;
            for (int i = 0; i < count; i++)
            {
                var unit = units[i];
                if (unit.currentlyExecuting != null && IsOrderOfType(unit.currentlyExecuting, structureType))
                {
                    counted++;
                    continue;
                }

                if (unit.currentOrder != null && IsOrderOfType(unit.currentOrder, structureType))
                {
                    counted++;
                    continue;
                }

                var orders = unit.orders;
                var ordersCount = orders.Count;
                for (int j = 0; j < ordersCount; j++)
                {
                    if (IsOrderOfType(orders[j], structureType))
                    {
                        counted++;
                        break;
                    }
                }
            }

            return counted;
        }

        /// <summary>
        /// Whether the given order is a BuildStructure order of the specificed type.
        /// </summary>
        /// <param name="order">The order to evaluate.</param>
        /// <param name="structureType">The desired structure type.</param>
        /// <returns>True if the order is a BuildStructure order with a matching structure type.</returns>
        private bool IsOrderOfType(IOrder order, StructureType structureType)
        {
            if (order.orderType == OrderType.BuildStructure)
            {
                if (structureType == StructureType.Any || ((BuildStructureOrder)order).structureType == structureType)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets how many resources of the supplied type this AI currently possesses.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>How many resources the AI has of the given type.</returns>
        public int GetCurrentResource(ResourceType type)
        {
            return _currentResources.GetValueOrDefault(type);
        }

        /// <summary>
        /// Adds the amount of resources of the specified type to the AI.
        /// </summary>
        /// <param name="type">The type of resource to type.</param>
        /// <param name="quantity">How much of the given resource type to add.</param>
        public void AddResource(ResourceType type, int quantity)
        {
            if (!_currentResources.ContainsKey(type))
            {
                _currentResources.Add(type, 0);
            }

            _currentResources[type] += quantity;
        }

        /// <summary>
        /// Spends resources of the supplied cost type.
        /// </summary>
        /// <param name="cost"></param>
        public void SpendResources(ResourceCost cost)
        {
            SpendResources(cost.resources);
        }

        /// <summary>
        /// Spends resources corresponding to each of the supplied <seealso cref="ResourceCostItem"/>s.
        /// </summary>
        /// <param name="costs"></param>
        public void SpendResources(params ResourceCostItem[] costs)
        {
            for (int i = 0; i < costs.Length; i++)
            {
                SpendResource(costs[i].type, costs[i].quantity);
            }
        }

        /// <summary>
        /// Spends the given quantity of resources, of the supplied resource type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="quantity"></param>
        public void SpendResource(ResourceType type, int quantity)
        {
            _currentResources[type] = Mathf.Max(0, _currentResources[type] - quantity);
        }

        /// <summary>
        /// Returns true if the AI has enough resources to cover the supplied <seealso cref="ResourceCost"/>.
        /// </summary>
        /// <param name="cost"></param>
        /// <returns></returns>
        public bool HasResources(ResourceCost cost)
        {
            return HasResources(cost.resources);
        }

        /// <summary>
        /// Returns true if the AI has enough resources to cover each of the supplied <seealso cref="ResourceCostItem"/>s.
        /// </summary>
        /// <param name="costs"></param>
        /// <returns></returns>
        public bool HasResources(params ResourceCostItem[] costs)
        {
            for (int i = 0; i < costs.Length; i++)
            {
                if (GetCurrentResource(costs[i].type) < costs[i].quantity)
                {
                    // if the AI cannot cover a single resource cost item, then the AI does not have enough resources
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Adds an observation of the given entity to the AI's shared memory.
        /// </summary>
        /// <param name="entity"></param>
        public void AddObservation(IEntity entity)
        {
            AddObservation(new Observation(entity));
        }

        /// <summary>
        /// Adds the given observation to the AI's shared memory.
        /// </summary>
        /// <param name="observation"></param>
        public void AddObservation(Observation observation)
        {
            AddOrReplaceIfNewer(this.observations, observation);

            if (!IsEnemy(observation))
            {
                return;
            }

            // add to enemy observations list
            AddOrReplaceIfNewer(this.enemyObservations, observation);
        }

        /// <summary>
        /// Adds the given observation to the supplied list, if either the entity has not been observed previously, or if the observation is newer than the previous observation.
        /// </summary>
        /// <param name="list">The list to add the new observation to.</param>
        /// <param name="observation">The new observation.</param>
        private void AddOrReplaceIfNewer(IList<Observation> list, Observation observation)
        {
            var count = list.Count;
            for (int i = 0; i < count; i++)
            {
                var obs = list[i];
                if (obs.entity.id != observation.entity.id)
                {
                    // not same entity
                    continue;
                }

                if (observation.timestamp < obs.timestamp)
                {
                    // only replace if the incoming timestamp is newer
                    continue;
                }

                list[i] = observation;
                return;
            }

            // observation did not already exist, so add as new one
            list.Add(observation);
        }

        /// <summary>
        /// Gets whether the supplied observation entity is an enemy of this AI. Returns false also if the passed entity is not a killable entity.
        /// </summary>
        /// <param name="observation"></param>
        /// <returns>True if the observation's entity is an enemy of this AI.</returns>
        public bool IsEnemy(Observation observation)
        {
            var hasHealth = observation.GetEntity<IHasHealth>();
            if (hasHealth != null && IsEnemy(hasHealth))
            {
                // entity is enemy
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified other is allied.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool IsAllied(IHasHealth other)
        {
            return ReferenceEquals(other.controller, this);
        }

        /// <summary>
        /// Determines whether the specified other is enemy.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool IsEnemy(IHasHealth other)
        {
            return !IsAllied(other);
        }

        /// <summary>
        /// Returns the context object for this AI controller.
        /// </summary>
        /// <param name="aiId"></param>
        /// <returns></returns>
        public IAIContext GetContext(Guid aiId)
        {
            return _context;
        }
    }
}