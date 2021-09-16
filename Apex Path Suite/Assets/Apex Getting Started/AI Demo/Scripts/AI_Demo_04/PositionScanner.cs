namespace Apex.GettingStarted.AI
{
    using Apex.AI;
    using Serialization;
    using UnityEngine;

    public sealed class PositionScanner : ActionBase<PositionContext>
    {
        [ApexSerialization, FriendlyName("Sampling Range", "The range in which the Scanner will sample positions")]
        public float samplingRange = 12f;

        [ApexSerialization, FriendlyName("Sampling Density", "The density in which the Scanner will sample positions")]
        public float samplingDensity = 1.5f;

        // This will loop through all possible positions within the samplingRange and with a density equal to the samplingDensity
        public override void Execute(PositionContext context)
        {
            context.positions.Clear();

            var position = context.self.position;
            var halfSamplingRange = this.samplingRange * 0.5f;
            for (float x = -halfSamplingRange; x <= halfSamplingRange; x += this.samplingDensity)
            {
                for (float z = -halfSamplingRange; z <= halfSamplingRange; z += this.samplingDensity)
                {
                    var point = new Vector3(position.x + x, 0f, position.z + z);
                    UnityEngine.AI.NavMeshHit hit;
                    if (UnityEngine.AI.NavMesh.SamplePosition(point, out hit, this.samplingDensity * 0.5f, UnityEngine.AI.NavMesh.AllAreas))
                    {
                        context.positions.Add(hit.position);
                    }
                }
            }
        }
    }
}