namespace Apex.Demo.RTS.AI
{
    using Apex.AI;

    public sealed class ReturnHarvest : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            var c = (UnitContext)context;
            var unit = c.unit as WorkerUnit;
            if (unit == null)
            {
                return;
            }

            unit.ReturnHarvest();
        }
    }
}