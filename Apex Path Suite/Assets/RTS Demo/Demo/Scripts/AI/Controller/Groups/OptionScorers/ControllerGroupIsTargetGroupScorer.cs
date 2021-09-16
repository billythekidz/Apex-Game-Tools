namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;

    public sealed class ControllerGroupIsTargetGroupScorer : OptionScorerBase<UnitGroup>
    {
        [ApexSerialization]
        public float score = 10f;

        [ApexSerialization(defaultValue = false), FriendlyName("Not", "Set to true to inverse the logic of this scorer, e.g. instead of scoring when true, it scores when false.")]
        public bool not;

        public override float Score(IAIContext context, UnitGroup group)
        {
            var c = (ControllerContext)context;
            if (c.targetGroup == group)
            {
                return this.not ? 0f : this.score;
            }

            return this.not ? this.score : 0f;
        }
    }
}