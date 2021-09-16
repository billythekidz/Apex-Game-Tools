namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Apex.Serialization;
    using UnityEngine;

    public sealed class ControllerPositionProximityToNearestEnemyScorer : OptionScorerBase<Vector3>
    {
        [ApexSerialization(defaultValue = 1f)]
        public float factor = 1f;

        [ApexSerialization]
        public float maxScore = 100f;

        [ApexSerialization(defaultValue = false)]
        public bool reverse;

        public override float Score(IAIContext context, Vector3 option)
        {
            var c = (ControllerContext)context;
            var enemyObservations = c.controller.enemyObservations;
            var count = enemyObservations.Count;
            if (count == 0)
            {
                return 0f;
            }

            var shortest = float.MaxValue;
            Observation nearest = null;
            for (int i = 0; i < count; i++)
            {
                var obs = enemyObservations[i];
                var distance = (obs.position - option).sqrMagnitude;
                if (distance < shortest)
                {
                    shortest = distance;
                    nearest = obs;
                }
            }

            if (nearest == null)
            {
                return 0f;
            }

            var magnitude = (nearest.position - option).magnitude * this.factor;
            if (this.reverse)
            {
                return Mathf.Min(this.maxScore, magnitude);
            }

            return Mathf.Max(0f, this.maxScore - magnitude);
        }
    }
}