namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Apex.Serialization;
    using UnityEngine;

    public sealed class OrderTimestampScorer : OptionScorerBase<IOrder>
    {
        [ApexSerialization(defaultValue = 1f)]
        public float factor = 1f;

        [ApexSerialization]
        public float maxScore = 1000f;

        [ApexSerialization(defaultValue = false)]
        public bool reversed;

        public override float Score(IAIContext context, IOrder option)
        {
            var timestamp = option.timestamp * this.factor;
            return this.reversed ?
                Mathf.Max(0f, this.maxScore - timestamp) :
                Mathf.Min(this.maxScore, timestamp);
        }
    }
}