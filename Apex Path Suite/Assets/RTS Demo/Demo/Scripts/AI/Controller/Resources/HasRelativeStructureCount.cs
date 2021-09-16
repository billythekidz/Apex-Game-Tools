namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;

    public class HasRelativeStructureCount : ContextualScorerBase
    {
        [ApexSerialization]
        public StructureType structureType;

        [ApexSerialization]
        public StructureType relativeTo;

        [ApexSerialization]
        public CompareOperator comparison = CompareOperator.LessThan;

        [ApexSerialization(defaultValue = 1f)]
        public float threshold = 1f;

        [ApexSerialization(defaultValue = true)]
        public bool includeInOrders = true;

        public override float Score(IAIContext context)
        {
            var c = (ControllerContext)context;
            var structureCount = c.controller.GetStructureCount(this.structureType);
            var relativeCount = c.controller.GetStructureCount(this.relativeTo);
            if (this.includeInOrders)
            {
                structureCount += c.controller.GetStructureCountInOrders(this.structureType);
                relativeCount += c.controller.GetStructureCountInOrders(this.relativeTo);
            }

            var factor = relativeCount > 0 ? structureCount / relativeCount : structureCount;
            if (Utils.IsOperatorTrue(this.comparison, factor, this.threshold))
            {
                return this.score;
            }

            return 0f;
        }
    }
}