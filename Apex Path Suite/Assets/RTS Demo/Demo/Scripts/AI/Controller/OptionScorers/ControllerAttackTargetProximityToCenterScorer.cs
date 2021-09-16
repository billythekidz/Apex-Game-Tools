namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Apex.Serialization;
    using UnityEngine;

    public sealed class ControllerAttackTargetProximityToCenterScorer : OptionScorerBase<IHasHealth>
    {
        [ApexSerialization(defaultValue = 1f)]
        public float factor = 1f;

        [ApexSerialization]
        public float maxScore = 100f;

        public override float Score(IAIContext context, IHasHealth target)
        {
            var c = (ControllerContext)context;
            var distance = (c.center.position - target.position).magnitude * this.factor;
            return Mathf.Max(0f, this.maxScore - distance);
        }
    }
}