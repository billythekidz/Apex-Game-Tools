namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;

    public sealed class StagingPointRangeToAttackPointScorer : OptionScorerBase<StagingPoint>
    {
        [ApexSerialization(defaultValue = CompareOperator.GreaterThan), FriendlyName("Comparison", "Select what kind of comparison to do")]
        public CompareOperator comparison = CompareOperator.GreaterThan;

        [ApexSerialization, FriendlyName("Range", "A custom range to use for the evaluation")]
        public float range = 10f;

        [ApexSerialization]
        public float score = 100f;

        public override float Score(IAIContext context, StagingPoint option)
        {
            var distanceSqr = (option.attackPoint - option.position).sqrMagnitude;
            if (Utils.IsOperatorTrue(this.comparison, distanceSqr, (this.range * this.range)))
            {
                return this.score;
            }

            return 0f;
        }
    }
}