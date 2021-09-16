namespace Apex.Demo.RTS.AI
{
    using UnityEngine;

    /// <summary>
    /// Center structure responsible for spawning workers, and functions as a resource depot - where workers return gathered resources.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.SpawningStructureBase" />
    public sealed class Center : SpawningStructureBase
    {
        [SerializeField, Range(0.1f, 5f), Tooltip("The radius at which workers may return harvest to this center.")]
        private float _returnHarvestRadius = 3f;

        [SerializeField, Range(1, 10), Tooltip("How many workers that may return harvest simultaneously.")]
        private int _maxSimultaneousHarvesters = 8;

        private Vector3[] _harvestPositions;

        /// <summary>
        /// Gets the type of the structure.
        /// </summary>
        /// <value>
        /// The type of the structure.
        /// </value>
        public override StructureType structureType
        {
            get { return StructureType.Center; }
        }

        /// <summary>
        /// Gets the harvest positions - the positions at which workers may be at when harvesting.
        /// </summary>
        /// <value>
        /// The harvest positions.
        /// </value>
        public Vector3[] harvestPositions
        {
            get { return _harvestPositions; }
        }

        /// <summary>
        /// Gets the return harvest radius - the radius at which workers may return harvest to this center.
        /// </summary>
        /// <value>
        /// The return harvest radius.
        /// </value>
        public float returnHarvestRadius
        {
            get { return _returnHarvestRadius; }
        }

        /// <summary>
        /// Called by Unity when enabled.
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();

            // Initialize all the harvester positions
            _harvestPositions = new Vector3[_maxSimultaneousHarvesters];
            var angle = 360f / _maxSimultaneousHarvesters;
            for (int i = 0; i < _maxSimultaneousHarvesters; i++)
            {
                _harvestPositions[i] = CircleHelpers.GetPointOnCircle(this.position, _returnHarvestRadius, angle, i);
            }
        }

        /// <summary>
        /// Spawns a new worker, if possible.
        /// </summary>
        /// <returns></returns>
        public bool SpawnWorker()
        {
            return HandleSpawnUnit(UnitType.Worker);
        }

        /// <summary>
        /// Spawns the specified count of workers for starting with a number of workers.
        /// </summary>
        /// <param name="count">The count.</param>
        public void GetStartWorkers(int count)
        {
            UnitPoolManager.instance.GetStartWorkers(this.controller, count, _spawnDistance, _anglePerSpawn, _lastSpawnIndex++);
        }

        /// <summary>
        /// Gets the nearest harvest return position relative to the given unit.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public Vector3 GetHarvestReturnPosition(WorkerUnit unit)
        {
            if (unit == null)
            {
                return _harvestPositions[0];
            }

            // TODO: does not ensure that the harvest position is not already occupied
            var shortest = float.MaxValue;
            var nearest = _harvestPositions[0];
            for (int i = 1; i < _harvestPositions.Length; i++)
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

        public override bool ReceiveDamage(float damage)
        {
            var isDead = base.ReceiveDamage(damage);
            if (isDead)
            {
                // invoke death on the controller when the Center structure dies
                this.controller.OnDeath();
            }

            return isDead;
        }
    }
}