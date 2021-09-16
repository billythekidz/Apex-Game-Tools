namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Apex.Serialization;
    using UnityEngine;

    public sealed class ResourceDistanceToNearestWorkersResourceTarget : OptionScorerBase<IResource>
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
            Vector3? nearest = null;
            for (int i = 0; i < count; i++)
            {
                var worker = observations[i].GetEntity<WorkerUnit>();
                if (worker == null || worker.resourceTarget == null)
                {
                    // not a worker unit or unit does not have a resource target
                    continue;
                }

                var distance = (resource.position - worker.resourceTarget.position).sqrMagnitude;
                if (distance < shortest)
                {
                    shortest = distance;
                    nearest = worker.resourceTarget.position;
                }
            }

            if (!nearest.HasValue)
            {
                return 0f;
            }

            var magnitude = (nearest.Value - resource.position).magnitude * this.factor;
            return Mathf.Min(this.maxScore, magnitude);
        }
    }
}