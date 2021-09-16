namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Apex.Serialization;

    public sealed class HasStructureCount : ContextualScorerBase
    {
        [ApexSerialization]
        public StructureType structureType = StructureType.Any;

        [ApexSerialization]
        public CompareOperator comparison = CompareOperator.GreaterThan;

        [ApexSerialization]
        public int threshold = 10;

        [ApexSerialization(defaultValue = true)]
        public bool includeInOrders = true;

        public override float Score(IAIContext context)
        {
            if (this.structureType == StructureType.None)
            {
                return 0f;
            }

            var c = (ControllerContext)context;
            var count = c.controller.GetStructureCount(this.structureType);
            if (this.includeInOrders)
            {
                count += c.controller.GetStructureCountInOrders(this.structureType);
            }

            if (Utils.IsOperatorTrue(this.comparison, count, threshold))
            {
                return this.score;
            }

            return 0f;
        }
    }
}