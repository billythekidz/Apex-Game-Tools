namespace Apex.GettingStarted.AI
{
    using System;
    using System.Collections.Generic;
    using Apex.AI;
    using Apex.AI.Visualization;
    using UnityEngine;

    public sealed class PositionVisualizer : CustomGizmoVisualizerComponent<PositionScanner, IList<Vector3>>
    {
        [SerializeField, Tooltip("The size of the Gizmo spheres"), Range(0f, 1f)]
        private float _sphereSize = 0.2f;

        // Draws a sphere on all sampled positions
        protected override void DrawGizmos(IList<Vector3> data)
        {
            Gizmos.color = Color.cyan;
            var count = data.Count;
            for (int i = 0; i < count; i++)
            {
                Gizmos.DrawSphere(data[i], _sphereSize);
            }
        }

        protected override IList<Vector3> GetDataForVisualization(PositionScanner aiEntity, IAIContext context, Guid aiId)
        {
            // Returns the positions so we can use it in DrawGizmos
            return ((PositionContext)context).positions;
        }
    }
}