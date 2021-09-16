namespace Apex.Demo.RTS.AI
{
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// Represents worker units, able to harvest resources and construct structures.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.UnitBase" />
    public sealed class WorkerUnit : UnitBase
    {
        [Header("Worker Only")]
        [SerializeField, Range(1, 100), Tooltip("The maximum amount of resources that this unit can carry.")]
        private int _maxCarriableResources = 30;

        [SerializeField, Range(1f, 10f), Tooltip("The radius at which workers can harvest resources.")]
        private float _harvestRadius = 4f;

        [SerializeField, Range(0.5f, 3f), Tooltip("A small buffer added to the return harvest radius to ensure that workers get close enough to the center.")]
        private float _returnHarvestRadiusBuffer = 1.5f;

        [SerializeField, Tooltip("Transform at which doodahs are placed to show that workers are carrying resources.")]
        private Transform _doodahPlacement;

        private float _lastHarvest;
        private IDoodah _currentDoodah;

        /// <summary>
        /// Gets the type of the unit.
        /// </summary>
        /// <value>
        /// The type of the unit.
        /// </value>
        public override UnitType unitType
        {
            get { return UnitType.Worker; }
        }

        /// <summary>
        /// Gets the maximum amount of resources that this worker can carry.
        /// </summary>
        /// <value>
        /// The maximum carriable resources.
        /// </value>
        public int maxCarriableResources
        {
            get { return _maxCarriableResources; }
        }

        /// <summary>
        /// Gets the current amount of resources that this worker is carrying.
        /// </summary>
        /// <value>
        /// The current carried resources.
        /// </value>
        public int currentCarriedResources
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the radius at which this worker can harvest resources.
        /// </summary>
        /// <value>
        /// The harvest radius.
        /// </value>
        public float harvestRadius
        {
            get { return _harvestRadius; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is currently building.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is currently building; otherwise, <c>false</c>.
        /// </value>
        public bool isBuilding
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is currently fleeing.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is fleeing; otherwise, <c>false</c>.
        /// </value>
        public bool isFleeing
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the current resource target for this worker.
        /// </summary>
        /// <value>
        /// The resource target.
        /// </value>
        public IResource resourceTarget
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is idle. In addition to not moving, not executing an order and not being dead, workers also check whether they are building.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is idle; otherwise, <c>false</c>.
        /// </value>
        public override bool isIdle
        {
            get { return base.isIdle && !this.isBuilding; }
        }

        /// <summary>
        /// Constructs a structure of the given type where the unit is currently standing, if possible. There is a ccost associated with constructing which determines how long the unit is busy while constructing.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public bool BuildStructure(StructureType type)
        {
            if (this.isBuilding)
            {
                // already building
                return false;
            }

            // check whether the nearest structure cell is occupied or not
            var cell = this.controller.structureGrid.GetNearestCell(this.position);
            if (cell != null && cell.occupied)
            {
                return false;
            }

            // attempt to build the structure
            var result = StructurePoolManager.instance.BuildStructure(type, this);
            if (result)
            {
                _animator.SetTrigger("Build");
                this.isBuilding = true;
                var buildTime = CostHelper.GetCost(type).time;
                CoroutineHelper.instance.StartCoroutine(FinishBuilding(buildTime));
            }

            return result;
        }

        /// <summary>
        /// Called when the worker is done constructing, to free it up once more.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns></returns>
        private IEnumerator FinishBuilding(float duration)
        {
            yield return new WaitForSeconds(duration);
            this.isBuilding = false;
        }

        /// <summary>
        /// This worker harvests from its current resource target if possible. Harvesting is limited by the attack rate (attacks per second).
        /// </summary>
        public void Harvest()
        {
            var resource = this.resourceTarget;
            if (resource == null)
            {
                // unit has no resource target
                return;
            }

            var time = Time.timeSinceLevelLoad;
            if (time - _lastHarvest < 1f / _attacksPerSecond)
            {
                return;
            }

            _lastHarvest = time;

            if ((resource.position - this.position).sqrMagnitude > (_harvestRadius * _harvestRadius))
            {
                // resource is too far away
                return;
            }

            _animator.SetTrigger("Harvest");
            this.LookAt(resource.position);

            var resources = resource.Harvest();
            this.currentCarriedResources = Mathf.Min(_maxCarriableResources, this.currentCarriedResources + resources);

            if (_currentDoodah == null)
            {
                _currentDoodah = DoodahPoolManager.instance.SpawnDoodah(_doodahPlacement);
            }
        }

        /// <summary>
        /// Returns the currently carried harvest to the center structure, if possible.
        /// </summary>
        public void ReturnHarvest()
        {
            var returnHarvestRadius = controller.center.returnHarvestRadius + _returnHarvestRadiusBuffer; // add a small buffer to make sure that units get within range and don't stop too early
            if ((controller.position - this.position).sqrMagnitude > (returnHarvestRadius * returnHarvestRadius))
            {
                // center is too far away
                return;
            }

            this.LookAt(controller.position);
            controller.AddResource(ResourceType.Metal, this.currentCarriedResources);
            this.currentCarriedResources = 0;

            if (_currentDoodah != null)
            {
                DoodahPoolManager.instance.Return(_currentDoodah);
                _currentDoodah = null;
            }
        }
    }
}