namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Apex.Serialization;

    public sealed class CanAffordStructure : ContextualScorerBase
    {
        [ApexSerialization]
        public StructureType structureType = StructureType.Center;

        [ApexSerialization(defaultValue = false), FriendlyName("Not", "Set to true to inverse the logic of this scorer, e.g. instead of scoring when true, it scores when false.")]
        public bool not;

        public override float Score(IAIContext context)
        {
            var c = (ControllerContext)context;
            var cost = CostHelper.GetCost(this.structureType);
            if (c.controller.HasResources(cost))
            {
                return this.not ? 0f : this.score;
            }

            return this.not ? this.score : 0f;
        }
    }
}