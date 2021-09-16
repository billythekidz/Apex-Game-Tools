namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;
    using UnityEngine;

    public sealed class GroupUnitCountScorer : OptionScorerBase<UnitGroup>
    {
        [ApexSerialization(defaultValue = 1f)]
        public float factor = 1f;

        [ApexSerialization]
        public float maxScore = 100f;

        [ApexSerialization]
        public bool reversed;

        public override float Score(IAIContext context, UnitGroup group)
        {
            var count = group.count * this.factor;
            if (this.reversed)
            {
                return Mathf.Max(0f, this.maxScore - count);
            }

            return Mathf.Min(this.maxScore, count);
        }
    }
}