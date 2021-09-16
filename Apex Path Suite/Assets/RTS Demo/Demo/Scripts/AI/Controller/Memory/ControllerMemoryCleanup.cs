namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Apex.Serialization;
    using UnityEngine;

    public sealed class ControllerMemoryCleanup : ActionBase
    {
        [ApexSerialization]
        public float observationVisibilityThreshold = 1.5f;

        public override void Execute(IAIContext context)
        {
            var c = (ControllerContext)context;
            var observations = c.controller.observations;
            var count = observations.Count;
            if (count == 0)
            {
                return;
            }

            for (int i = count - 1; i >= 0; i--)
            {
                var obs = observations[i];
                if (obs.entity == null || !obs.entity.active)
                {
                    observations.RemoveAt(i);                   
                }
            }

            var enemyObservations = c.controller.enemyObservations;
            var enemyCount = enemyObservations.Count;
            if (enemyCount == 0)
            {
                return;
            }

            for (int i = enemyCount - 1; i >= 0; i--)
            {
                var obs = enemyObservations[i];
                if (obs.entity == null || !obs.entity.active)
                {
                    enemyObservations.RemoveAt(i);
                }
            }

            // Clean up empty groups => stop iterating at 1 to avoid removing default group
            var groups = c.controller.groups;
            var gcount = groups.Count;
            for (int i = gcount - 1; i > 0; i--)
            {
                if (groups[i].count == 0)
                {
                    c.controller.groups.RemoveAt(i);
                }
            }
        }
    }
}