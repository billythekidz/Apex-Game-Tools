namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Apex.Serialization;
    using UnityEngine;

    public sealed class OrderPriorityScorer : OptionScorerBase<IOrder>
    {
        [ApexSerialization(defaultValue = 1f)]
        public float factor = 1f;

        [ApexSerialization]
        public float maxScore = 100000f;

        public override float Score(IAIContext context, IOrder option)
        {
            return Mathf.Min(this.maxScore, option.priority * this.factor);
        }
    }
}