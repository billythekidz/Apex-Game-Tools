namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;
    using UnityEngine;

    public sealed class ProximityToTemporaryTargetPositionScorer : OptionScorerBase<Vector3>
    {
        [ApexSerialization(defaultValue = 1f)]
        public float factor = 1f;

        [ApexSerialization]
        public float maxScore = 100f;

        public override float Score(IAIContext context, Vector3 position)
        {
            var c = (UnitContext)context;
            var tempTarget = c.temporaryTarget;
            if (tempTarget == null)
            {
                return 0f;
            }

            var distance = (tempTarget.position - position).magnitude * this.factor;
            return Mathf.Max(0f, this.maxScore - distance);
        }
    }
}