using UnityEngine;

namespace Apex.Demo.RTS.AI
{
    

    /// <summary>
    /// Base class for all harvestable resources.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.EntityBase" />
    /// <seealso cref="Apex.Demo.RTS.AI.IResource" />
    [RequireComponent(typeof(UnityEngine.AI.NavMeshObstacle))]
    public abstract class ResourceBase : EntityBase, IResource
    {
        [SerializeField, Range(2, 10000), Tooltip("The maximum amount of resources to start with.")]
        private int _maxResources = 1000;

        [SerializeField, Range(1, 1000), Tooltip("The mimum amount of resources to start with.")]
        private int _minResources = 100;

        [SerializeField, Range(1, 100), Tooltip("How many resources a harvester can get at maximum per harvest.")]
        private int _maxResourcesPerHarvest = 10;

        [SerializeField, Range(0.1f, 5f), Tooltip("The minimum radius at which this may be harvested.")]
        private float _minHarvestRadius = 2f;

        [SerializeField, Range(1, 10), Tooltip("The maximum amount of workers that can simultaneously harvest.")]
        private int _maxSimultaneousHarvesters = 5;

        private Collider _collider;
        private Vector3[] _harvestPositions;

        /// <summary>
        /// Gets the type of the resource.
        /// </summary>
        /// <value>
        /// The type of the resource.
        /// </value>
        public abstract ResourceType resourceType { get; }

        /// <summary>
        /// Gets the type of the entity.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        public override EntityType entityType
        {
            get { return EntityType.Resource; }
        }

        /// <summary>
        /// Gets the current amount of resources.
        /// </summary>
        /// <value>
        /// The current resources.
        /// </value>
        public int currentResources
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the cached collider.
        /// </summary>
        /// <value>
        /// The cached collider.
        /// </value>
        public Collider cachedCollider
        {
            get { return _collider ?? (_collider = this.GetComponent<Collider>()); }
        }

        /// <summary>
        /// Gets the harvest positions - the positions from where workers may harvest this resource.
        /// </summary>
        /// <value>
        /// The harvest positions.
        /// </value>
        public Vector3[] harvestPositions
        {
            get { return _harvestPositions; }
        }

        /// <summary>
        /// Gets the name when mouse hovers above.
        /// </summary>
        /// <value>
        /// The name when mouse hovers above.
        /// </value>
        public override string mouseOverName
        {
            get { return this.resourceType.ToString(); }
        }

        /// <summary>
        /// Called by Unity when enabled.
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();

            // Start resource with a random amount of resources (within ranges) and a random rotation
            this.currentResources = Random.Range(_minResources, _maxResources);
            this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, Random.Range(0f, 360f), this.transform.eulerAngles.z);

            // Initialize all the harvester positions
            _harvestPositions = new Vector3[_maxSimultaneousHarvesters];
            var angle = 360f / _maxSimultaneousHarvesters;
            for (int i = 0; i < _maxSimultaneousHarvesters; i++)
            {
                _harvestPositions[i] = CircleHelpers.GetPointOnCircle(this.position, _minHarvestRadius, angle, i);
            }
        }

        /// <summary>
        /// Gets the nearest harvest position compared to the given unit's position.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public Vector3 GetHarvestPosition(WorkerUnit unit)
        {
            var shortest = float.MaxValue;
            var nearest = this.transform.position;
            for (int i = 0; i < _harvestPositions.Length; i++)
            {
                var distance = (_harvestPositions[i] - unit.position).sqrMagnitude;
                if (distance < shortest)
                {
                    nearest = _harvestPositions[i];
                    shortest = distance;
                }
            }

            return nearest;
        }

        /// <summary>
        /// Harvests this resource for a variable amount of resources.
        /// </summary>
        /// <returns>The amount of resources harvested.</returns>
        public int Harvest()
        {
            var resources = Mathf.Min(this.currentResources, Random.Range(1, _maxResourcesPerHarvest));
            this.currentResources -= resources;

            if (this.currentResources <= 0)
            {
                // clear up a grid cell in the structure grid, if one was occupied before
                var controllers = FindObjectsOfType<AIController>();
                for (int i = 0; i < controllers.Length; i++)
                {
                    var cell = controllers[i].structureGrid.GetCellIn(this.cachedCollider.bounds);
                    if (cell == null || !cell.occupied)
                    {
                        continue;
                    }

                    cell.occupied = false;
                }

                // deactivate this resource component
                this.active = false;
            }

            return resources;
        }
    }
}