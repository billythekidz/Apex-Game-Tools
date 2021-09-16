namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;

    public sealed class HasCurrentOrderOfCategory : ContextualScorerBase
    {
        [ApexSerialization]
        public OrderCategoryType orderCategory = OrderCategoryType.Any;

        [ApexSerialization(defaultValue = false), FriendlyName("Not", "Set to true to inverse the logic of this scorer, e.g. instead of scoring when true, it scores when false.")]
        public bool not;

        public override float Score(IAIContext context)
        {
            var c = (UnitContext)context;
            var order = c.unit.currentOrder;

            if (order != null && (this.orderCategory == OrderCategoryType.Any || order.orderCategory == this.orderCategory))
            {
                return this.not ? 0f : this.score;
            }

            return this.not ? this.score : 0f;
        }
    }
}