namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;

    public sealed class AttackPointRangeToCenterScorer : OptionScorerBase<AttackPoint>
    {
        [ApexSerialization(defaultValue = CompareOperator.GreaterThan), FriendlyName("Comparison", "Select what kind of comparison to do")]
        public CompareOperator comparison = CompareOperator.GreaterThan;

        [ApexSerialization, FriendlyName("Range", "A custom range to use for the evaluation")]
        public float range = 50f;

        [ApexSerialization]
        public float score = 1000f;

        public override float Score(IAIContext context, AttackPoint option)
        {
            var c = (ControllerContext)context;
            var distanceSqr = (c.center.position - option.position).sqrMagnitude;
            if (Utils.IsOperatorTrue(this.comparison, distanceSqr, (this.range * this.range)))
            {
                return this.score;
            }

            return 0f;
        }
    }
}