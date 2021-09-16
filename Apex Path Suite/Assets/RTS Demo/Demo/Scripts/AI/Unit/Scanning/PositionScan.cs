namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Apex.Serialization;
    using UnityEngine;

    public sealed class PositionScan : ActionBase
    {
        [ApexSerialization(defaultValue = 1f)]
        public float allowedImprecision = 1f;

        [ApexSerialization(defaultValue = 2f)]
        public float positionSpacing = 2f;

        [ApexSerialization(defaultValue = 1.5f)]
        public float samplingFactor = 1.5f;

        public override void Execute(IAIContext context)
        {
            var c = (UnitContext)context;

            // clear old positions
            c.sampledPositions.Clear();

            var mask = c.unit.areaMask;
            var pos = c.unit.position;
            var radius = c.unit.scanRadius * Mathf.Max(1f, this.samplingFactor);

            for (float x = pos.x - radius; x <= pos.x + radius; x += this.positionSpacing)
            {
                for (float z = pos.z - radius; z <= pos.z + radius; z += this.positionSpacing)
                {
                    UnityEngine.AI.NavMeshHit hit;
                    if (UnityEngine.AI.NavMesh.SamplePosition(new Vector3(x, pos.y, z), out hit, this.allowedImprecision, mask))
                    {
                        // Only positions that are clear on the NavMesh are added to the sampled positions, there is no reason to store blocked or unreachable positions
                        c.sampledPositions.Add(hit.position);
                    }
                }
            }
        }
    }
}