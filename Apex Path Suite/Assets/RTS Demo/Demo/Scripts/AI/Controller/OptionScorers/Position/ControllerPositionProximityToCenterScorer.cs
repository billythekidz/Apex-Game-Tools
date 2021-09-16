namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Apex.Serialization;
    using UnityEngine;

    public sealed class ControllerPositionProximityToCenterScorer : OptionScorerBase<Vector3>
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
            var distance = (c.center.position - option).magnitude * this.factor;
            if (this.reverse)
            {
                return Mathf.Min(this.maxScore, distance);
            }

            return Mathf.Max(0f, this.maxScore - distance);
        }
    }
}