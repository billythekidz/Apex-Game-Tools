namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Apex.Serialization;
    using UnityEngine;

    public sealed class AttackTargetProximityScorer : OptionScorerBase<IHasHealth>
    {
        [ApexSerialization]
        public float factor = 0.1f;

        [ApexSerialization]
        public float maxScore = 100f;

        public override float Score(IAIContext context, IHasHealth target)
        {
            var c = (UnitContext)context;
            var distance = (c.unit.position - target.position).magnitude * this.factor;
            return Mathf.Max(0f, this.maxScore - distance);
        }
    }
}