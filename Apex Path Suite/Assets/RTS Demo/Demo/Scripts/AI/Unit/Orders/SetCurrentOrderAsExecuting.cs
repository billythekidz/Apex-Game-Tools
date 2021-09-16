namespace Apex.Demo.RTS.AI
{
    using Apex.AI;

    public sealed class SetCurrentOrderAsExecuting : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            var c = (UnitContext)context;
            var currentOrder = c.unit.currentOrder;
            if (currentOrder == null)
            {
                return;
            }

            c.unit.currentlyExecuting = currentOrder;
        }
    }
}