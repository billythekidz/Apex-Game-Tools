namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;
    using UnityEngine;

    public sealed class DistanceToNearestEnemyPositionScorer : OptionScorerBase<Vector3>
    {
        [ApexSerialization(defaultValue = 1f)]
        public float factor = 1f;

        [ApexSerialization]
        public float maxScore = 100f;

        public override float Score(IAIContext context, Vector3 position)
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
                var hasHealth = obs.GetEntity<IHasHealth>();
                if (hasHealth == null)
                {
                    // observation does not have health and thus cannot be an enemy
                    continue;
                }

                var distanceSqr = (hasHealth.position - position).sqrMagnitude;
                if (distanceSqr < shortest)
                {
                    nearest = hasHealth;
                    shortest = distanceSqr;
                }
            }

            if (nearest == null)
            {
                return 0f;
            }

            var distance = (nearest.position - position).magnitude * this.factor;
            return Mathf.Min(this.maxScore, distance);
        }
    }
}