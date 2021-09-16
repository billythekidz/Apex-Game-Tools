namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Apex.Serialization;
    using UnityEngine;

    public sealed class ResourceDistanceToNearestEnemy : OptionScorerBase<IResource>
    {
        [ApexSerialization(defaultValue = 1f)]
        public float factor = 1f;

        [ApexSerialization]
        public float maxScore = 100f;

        [ApexSerialization(defaultValue = false)]
        public bool ignoreStructures;

        public override float Score(IAIContext context, IResource resource)
        {
            var c = (UnitContext)context;
            var observations = c.unit.enemyObservations;
            var count = observations.Count;
            if (count == 0)
            {
                return 0f;
            }

            var shortest = float.MaxValue;
            IHasHealth nearest = null;
            for (int i = 0; i < count; i++)
            {
                var obs = observations[i];
                if (this.ignoreStructures && obs.entity.entityType == EntityType.Structure)
                {
                    continue;
                }

                var hasHealth = obs.GetEntity<IHasHealth>();
                if (hasHealth == null)
                {
                    // not a unit or structure
                    continue;
                }

                var distance = (resource.position - obs.position).sqrMagnitude;
                if (distance < shortest)
                {
                    shortest = distance;
                    nearest = hasHealth;
                }
            }

            if (nearest == null)
            {
                return 0f;
            }

            var magnitude = (nearest.position - resource.position).magnitude * this.factor;
            return Mathf.Min(this.maxScore, magnitude);
        }
    }
}