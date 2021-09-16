namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Apex.Serialization;
    using UnityEngine;

    public sealed class StructureAttackTargetProximityScorer : OptionScorerBase<IHasHealth>
    {
        [ApexSerialization]
        public float factor = 0.1f;

        [ApexSerialization]
        public float maxScore = 100f;

        public override float Score(IAIContext context, IHasHealth target)
        {
            var c = (StructureContext)context;
            var distance = (c.structure.position - target.position).magnitude * this.factor;
            return Mathf.Max(0f, this.maxScore - distance);
        }
    }
}