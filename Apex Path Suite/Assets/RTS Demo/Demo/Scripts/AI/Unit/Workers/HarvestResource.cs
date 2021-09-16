namespace Apex.Demo.RTS.AI
{
    using Apex.AI;

    public sealed class HarvestResource : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            var c = (UnitContext)context;
            var unit = c.unit as WorkerUnit;
            if (unit == null)
            {
                // only worker units can harvest
                return;
            }

            if (unit.resourceTarget == null)
            {
                // unit has no resource target
                return;
            }

            unit.Harvest();
        }
    }
}