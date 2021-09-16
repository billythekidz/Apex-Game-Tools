namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Apex.Serialization;

    public sealed class HasOrder : ContextualScorerBase
    {
        [ApexSerialization]
        public OrderType orderType = OrderType.Any;

        [ApexSerialization(defaultValue = false), FriendlyName("Not", "Set to true to inverse the logic of this scorer, e.g. instead of scoring when true, it scores when false.")]
        public bool not;

        public override float Score(IAIContext context)
        {
            var c = (UnitContext)context;
            var orders = c.unit.orders;
            var count = orders.Count;
            if (count == 0)
            {
                return this.not ? this.score : 0f;
            }

            if (this.orderType == OrderType.Any)
            {
                return this.not ? 0f : this.score;
            }

            for (int i = 0; i < count; i++)
            {
                if (orders[i].orderType == this.orderType)
                {
                    return this.not ? 0f : this.score;
                }
            }

            return this.not ? this.score : 0f;
        }
    }
}