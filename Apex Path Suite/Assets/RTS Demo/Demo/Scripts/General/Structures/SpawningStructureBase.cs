namespace Apex.Demo.RTS.AI
{
    using UnityEngine;

    /// <summary>
    /// Abstract base class for all structures that can spawn units.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.StructureBase" />
    public abstract class SpawningStructureBase : StructureBase
    {
        [Header("Spawning")]
        [SerializeField, Range(1f, 12f), Tooltip("How far away units are spawned from this structure.")]
        protected float _spawnDistance = 6f;

        [SerializeField, Range(5f, 90f), Tooltip("How large an angle there is between spawn positions for units.")]
        protected float _anglePerSpawn = 35f;

        protected float _lastSpawn;
        protected int _lastSpawnIndex;

        /// <summary>
        /// Gets a value indicating whether this structure is in cooldown.
        /// </summary>
        /// <value>
        ///   <c>true</c> if in cooldown; otherwise, <c>false</c>.
        /// </value>
        public bool inCooldown
        {
            get { return Time.timeSinceLevelLoad < _lastSpawn; }
        }

        /// <summary>
        /// Handles the spawning of a unit of the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the spawning succeeded, <c>false</c> otherwise.
        /// </returns>
        protected bool HandleSpawnUnit(UnitType type)
        {
            // only valid types
            if (type == UnitType.None || type == UnitType.Any)
            {
                Debug.LogError(this.ToString() + " cannot spawn units of type (not supported): " + type);
                return false;
            }

            // spawn cooldown
            var time = Time.timeSinceLevelLoad;
            if (time < _lastSpawn)
            {
                return false;
            }

            // cost/resource
            var costs = CostHelper.GetCost(type);
            if (!this.controller.HasResources(costs))
            {
                // cannot afford
                return false;
            }

            // increment next spawning time
            _lastSpawn = time + costs.time;

            // actually spend resources
            this.controller.SpendResources(costs);

            // actual spawning
            return UnitPoolManager.instance.SpawnUnit(type, this.controller, this.position, _spawnDistance, _anglePerSpawn, _lastSpawnIndex++);
        }
    }
}