namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Apex.Serialization;

    public sealed class CompleteCurrentlyExecutingOrder : ActionBase
    {
        [ApexSerialization(defaultValue = true)]
        public bool nullCurrentOrderAlso = true;

        public override void Execute(IAIContext context)
        {
            var c = (UnitContext)context;
            var unit = c.unit;

            if (this.nullCurrentOrderAlso)
            {
                unit.currentOrder = null;
            }

            if (unit.currentlyExecuting != null)
            {
                unit.completedOrders.Add(unit.currentlyExecuting);
                unit.currentlyExecuting = null;
            }
        }
    }
}