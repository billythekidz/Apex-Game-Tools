using UnityEngine;

namespace Apex.Demo.RTS.AI
{
    

    /// <summary>
    /// Abstract base class component for all structures.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.HasHealthBase" />
    /// <seealso cref="Apex.Demo.RTS.AI.IStructure" />
    [RequireComponent(typeof(UnityEngine.AI.NavMeshObstacle))]
    public abstract class StructureBase : HasHealthBase, IStructure
    {
        [SerializeField, Range(1f, 100f), Tooltip("How prioritized this structure is in regards to threat calculations.")]
        private float _threatPriority = 1f;

        /// <summary>
        /// Gets the type of the structure.
        /// </summary>
        /// <value>
        /// The type of the structure.
        /// </value>
        public abstract StructureType structureType { get; }

        /// <summary>
        /// Gets the type of the entity.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        public override EntityType entityType
        {
            get { return EntityType.Structure; }
        }

        /// <summary>
        /// Gets or sets the threat score - how threatened this structure is analyzed to be in the eyes of the owning AI controller.
        /// </summary>
        /// <value>
        /// The threat score.
        /// </value>
        public float threatScore
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the threat priority - how prioritized this structure is set to be.
        /// </summary>
        /// <value>
        /// The threat priority.
        /// </value>
        public float threatPriority
        {
            get { return _threatPriority; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is ready, e.g. to attack or spawn units.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is ready; otherwise, <c>false</c>.
        /// </value>
        public bool isReady
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the name when mouse hovers above.
        /// </summary>
        /// <value>
        /// The name when mouse hovers above.
        /// </value>
        public override string mouseOverName
        {
            get { return this.structureType.ToString(); }
        }

        /// <summary>
        /// Called by Unity when enabled.
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            this.currentHealth = _maxHealth;
        }

        /// <summary>
        /// Receive the specified amount of damage.
        /// </summary>
        /// <param name="damage">The damage.</param>
        /// <returns>
        /// True if the entity is dead, false otherwise
        /// </returns>
        public override bool ReceiveDamage(float damage)
        {
            if (this.isDead)
            {
                return true;
            }

            var lookUp = Quaternion.LookRotation(Vector3.up);
            ParticlePoolManager.instance.SpawnParticleSystem(ParticlesType.AreaImpact, this.position + Vector3.up, lookUp);
            this.controller.lastAttack = Time.timeSinceLevelLoad;

            this.currentHealth -= damage;
            if (this.currentHealth <= 0f)
            {
                // OnDeath
                if (this.controller != null && this.controller.structures != null)
                {
                    this.controller.structures.Remove(this);
                }

                DecalPoolManager.instance.SpawnDecal(DecalType.ExplostionLarge, this.position);
                ParticlePoolManager.instance.SpawnParticleSystem(ParticlesType.VeryBigExplosion, this.position + Vector3.up, lookUp);
                ParticlePoolManager.instance.SpawnParticleSystem(ParticlesType.SparksBig, this.position, Quaternion.identity);

                ReturnStructure();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns this structure to the pool from whence it came. Also unoccupies a structure grid cell.
        /// </summary>
        private void ReturnStructure()
        {
            if (this.controller != null && this.controller.structureGrid != null)
            {
                // a spot in the controller's structure grid is now vacant
                var nearest = this.controller.structureGrid.GetNearestCell(this.position);
                nearest.occupied = false;
            }

            StructurePoolManager.instance.Return(this);
        }
    }
}