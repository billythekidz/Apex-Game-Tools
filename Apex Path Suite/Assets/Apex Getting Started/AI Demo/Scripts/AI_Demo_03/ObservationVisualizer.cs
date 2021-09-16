namespace Apex.GettingStarted.AI
{
    using System;
    using Apex.AI;
    using Apex.AI.Visualization;
    using UnityEngine;

    public sealed class ObservationVisualizer : CustomGizmoVisualizerComponent<EntityScanner, ObservationContext>
    {
        protected override void DrawGizmos(ObservationContext data)
        {
            var pos = data.self.transform.position;
            var observations = data.observations;
            var count = observations.Count;
            for (int i = 0; i < count; i++)
            {
                Gizmos.DrawLine(pos, observations[i].gameObject.transform.position);
            }
        }

        // This method will decide what data we send to the actual draw method
        protected override ObservationContext GetDataForVisualization(EntityScanner aiEntity, IAIContext c, Guid aiId)
        {
            return (ObservationContext)c;
        }
    }
}