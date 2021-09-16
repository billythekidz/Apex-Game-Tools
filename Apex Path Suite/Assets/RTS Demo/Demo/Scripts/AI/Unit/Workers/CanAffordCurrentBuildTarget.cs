namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;

    public sealed class CanAffordCurrentBuildTarget : ContextualScorerBase
    {
        [ApexSerialization(defaultValue = false), FriendlyName("Not", "Set to true to inverse the logic of this scorer, e.g. instead of scoring when true, it scores when false.")]
        public bool not;

        public override float Score(IAIContext context)
        {
            var c = (UnitContext)context;
            var buildTarget = c.buildTarget;
            if (buildTarget == null)
            {
                return 0f;
            }

            var cost = CostHelper.GetCost(buildTarget.type);
            if (c.unit.controller.HasResources(cost))
            {
                return this.not ? 0f : this.score;
            }

            return this.not ? this.score : 0f;
        }
    }
}