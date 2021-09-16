namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Apex.Serialization;

    public sealed class HasResource : ContextualScorerBase
    {
        [ApexSerialization]
        public ResourceType resourceType = ResourceType.Any;

        [ApexSerialization]
        public int threshold = 10;

        [ApexSerialization]
        public CompareOperator comparison = CompareOperator.GreaterThanOrEquals;

        public override float Score(IAIContext context)
        {
            var c = (ControllerContext)context;
            var resources = c.controller.GetCurrentResource(this.resourceType);
            if (Utils.IsOperatorTrue(this.comparison, resources, this.threshold))
            {
                return this.score;
            }

            return 0f;
        }
    }
}