namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;
    using UnityEngine;

    public sealed class PositionProximityToMapCenterScorer : OptionScorerBase<Vector3>
    {
        [ApexSerialization(defaultValue = 1f)]
        public float factor = 1f;

        [ApexSerialization]
        public float maxScore = 100f;

        public override float Score(IAIContext context, Vector3 option)
        {
            return Mathf.Max(0f, this.maxScore - (option.magnitude * this.factor));
        }
    }
}