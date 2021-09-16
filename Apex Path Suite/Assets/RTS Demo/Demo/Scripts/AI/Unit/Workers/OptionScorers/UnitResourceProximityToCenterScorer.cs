namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;
    using UnityEngine;

    public sealed class UnitResourceProximityToCenterScorer : OptionScorerBase<IResource>
    {
        [ApexSerialization(defaultValue = 1f)]
        public float factor = 1f;

        [ApexSerialization]
        public float maxScore = 100f;

        public override float Score(IAIContext context, IResource resource)
        {
            var c = (UnitContext)context;
            var distance = (c.unit.controller.center.position - resource.position).magnitude * this.factor;
            return Mathf.Max(0f, this.maxScore - distance);
        }
    }
}