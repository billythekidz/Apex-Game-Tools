namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Apex.Serialization;
    using UnityEngine;

    public sealed class ResourceDistanceToNearestWorker : OptionScorerBase<IResource>
    {
        [ApexSerialization(defaultValue = 1f)]
        public float factor = 1f;

        [ApexSerialization]
        public float maxScore = 100f;

        public override float Score(IAIContext context, IResource resource)
        {
            var c = (UnitContext)context;
            var observations = c.unit.observations;
            var count = observations.Count;
            if (count == 0)
            {
                return 0f;
            }

            var shortest = float.MaxValue;
            WorkerUnit nearest = null;
            for (int i = 0; i < count; i++)
            {
                var obs = observations[i];

                var worker = obs.GetEntity<WorkerUnit>();
                if (worker == null)
                {
                    // not a worker unit
                    continue;
                }

                var distance = (resource.position - obs.position).sqrMagnitude;
                if (distance < shortest)
                {
                    shortest = distance;
                    nearest = worker;
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