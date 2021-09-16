namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Apex.Serialization;
    using UnityEngine;

    public sealed class ResourceRichnessScorer : OptionScorerBase<IResource>
    {
        [ApexSerialization]
        public float maxScore = 100f;

        [ApexSerialization(defaultValue = 1f)]
        public float factor = 1f;

        public override float Score(IAIContext context, IResource resource)
        {
            return Mathf.Min(this.maxScore, resource.currentResources * this.factor);
        }
    }
}