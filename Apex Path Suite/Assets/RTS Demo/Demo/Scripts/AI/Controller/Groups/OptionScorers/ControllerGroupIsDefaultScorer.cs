namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;

    public sealed class ControllerGroupIsDefaultScorer : OptionScorerBase<UnitGroup>
    {
        [ApexSerialization]
        public float score;

        [ApexSerialization(defaultValue = false)]
        public bool not;

        public override float Score(IAIContext context, UnitGroup group)
        {
            var c = (ControllerContext)context;
            if (c.controller.defaultGroup == group)
            {
                return this.not ? 0f : this.score;
            }

            return this.not ? this.score : 0f;
        }
    }
}