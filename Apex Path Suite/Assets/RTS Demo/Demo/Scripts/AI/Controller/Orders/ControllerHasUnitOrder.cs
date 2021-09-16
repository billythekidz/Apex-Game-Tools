namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;

    public sealed class ControllerHasUnitOrder : ContextualScorerBase
    {
        [ApexSerialization]
        public OrderType orderType = OrderType.Any;

        [ApexSerialization(defaultValue = false), FriendlyName("Not", "Set to true to inverse the logic of this scorer, e.g. instead of scoring when true, it scores when false.")]
        public bool not;

        public override float Score(IAIContext context)
        {
            var c = (ControllerContext)context;
            var units = c.controller.units;
            var count = units.Count;
            for (int i = 0; i < count; i++)
            {
                var unit = units[i];
                if (unit.currentlyExecuting != null)
                {
                    if (this.orderType == OrderType.Any || unit.currentlyExecuting.orderType == this.orderType)
                    {
                        return this.not ? 0f : this.score;
                    }
                }

                if (unit.currentOrder != null)
                {
                    if (this.orderType == OrderType.Any || unit.currentOrder.orderType == this.orderType)
                    {
                        return this.not ? 0f : this.score;
                    }
                }

                var orders = unit.orders;
                var ocount = orders.Count;
                for (int j = 0; j < ocount; j++)
                {
                    var order = orders[j];
                    if (this.orderType == OrderType.Any || order.orderType == this.orderType)
                    {
                        return this.not ? 0f : this.score;
                    }
                }
            }

            return this.not ? this.score : 0f;
        }
    }
}