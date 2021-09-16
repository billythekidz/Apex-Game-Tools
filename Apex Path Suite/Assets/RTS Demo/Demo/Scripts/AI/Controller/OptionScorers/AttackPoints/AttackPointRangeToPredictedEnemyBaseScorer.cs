namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;

    public sealed class AttackPointRangeToPredictedEnemyBaseScorer : OptionScorerBase<AttackPoint>
    {
        [ApexSerialization(defaultValue = CompareOperator.GreaterThan), FriendlyName("Comparison", "Select what kind of comparison to do")]
        public CompareOperator comparison = CompareOperator.GreaterThan;

        [ApexSerialization, FriendlyName("Range", "A custom range to use for the evaluation")]
        public float range = 10f;

        [ApexSerialization]
        public float score = 100f;

        public override float Score(IAIContext context, AttackPoint option)
        {
            var c = (ControllerContext)context;
            var distanceSqr = (c.controller.predictedEnemyBasePosition - option.position).sqrMagnitude;
            if (Utils.IsOperatorTrue(this.comparison, distanceSqr, (this.range * this.range)))
            {
                return this.score;
            }

            return 0f;
        }
    }
}