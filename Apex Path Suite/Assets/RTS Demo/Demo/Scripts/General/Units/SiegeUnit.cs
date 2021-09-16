namespace Apex.Demo.RTS.AI
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Represents siege units. Siege units have a special attack behaviour, in that they have a minimum radius for their attacks, as well as a maximum radius.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.UnitBase" />
    public sealed class SiegeUnit : UnitBase
    {
        [Header("Siege Only")]
        [SerializeField, Range(1f, 25f), Tooltip("The minimum radius at which siege units may attack targets.")]
        private float _minimumAttackRadius = 5f;

        /// <summary>
        /// Gets the type of the unit.
        /// </summary>
        /// <value>
        /// The type of the unit.
        /// </value>
        public override UnitType unitType
        {
            get { return UnitType.Siege; }
        }

        /// <summary>
        /// Gets the minimum attack radius.
        /// </summary>
        /// <value>
        /// The minimum attack radius.
        /// </value>
        public float minimumAttackRadius
        {
            get { return _minimumAttackRadius; }
        }

        /// <summary>
        /// The internal attack method. Siege units can only hit targets that are in front of them, and at a distance between the set minimum and maximum radius.
        /// </summary>
        /// <param name="dmg">The DMG.</param>
        protected override void InternalAttack(float dmg)
        {
            var layers = LayersHelper.instance;
            var hits = Utils.hitsBuffer;

            var pos = this.position + this.transform.forward * (_unitRadius + _minimumAttackRadius);
            Physics.SphereCastNonAlloc(pos, _unitRadius * 2f, this.transform.forward, hits, _attackRadius, layers.unitLayer | layers.structureLayer);

            // Sort the results to ensure that the closest hit is actually damaged
            _comparer.position = this.position;
            Array.Sort(hits, _comparer);

            for (int i = 0; i < hits.Length; i++)
            {
                var hit = hits[i];
                if (hit.transform == null)
                {
                    continue;
                }

                if (hit.transform == this.transform)
                {
                    // ignore hits with self
                    continue;
                }

                var hasHealth = hit.collider.GetEntity<IHasHealth>();
                if (hasHealth == null || hasHealth.isDead)
                {
                    // hit is invalid or dead - ignore
                    continue;
                }

                if (IsAlly(hasHealth))
                {
                    // hit is an ally, ignore
                    continue;
                }

                // apply damage to single (first) hit
                hasHealth.lastAttacker = this;
                hasHealth.ReceiveDamage(dmg);
                break;
            }

            // spawn particle effect systems
            ParticlePoolManager.instance.SpawnParticleSystem(ParticlesType.BigMuzzle, this.position);
            ParticlePoolManager.instance.SpawnParticleSystem(ParticlesType.SmallSmoke, this.position);
        }
    }
}