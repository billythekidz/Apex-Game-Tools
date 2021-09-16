namespace Apex.Demo.RTS.AI
{
    using Apex.AI;

    public sealed class SetBestOrderToCurrentOrder : ActionWithOptions<IOrder>
    {
        public override void Execute(IAIContext context)
        {
            var c = (UnitContext)context;
            var orders = c.unit.orders;
            if (orders.Count == 0)
            {
                return;
            }

            var best = this.GetBest(c, orders);
            if (best == null)
            {
                return;
            }

            c.unit.currentOrder = best;
            c.unit.orders.Remove(best);
        }
    }
}