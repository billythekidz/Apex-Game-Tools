namespace Apex.Demo.RTS.AI
{
    using Apex.AI;

    public sealed class NullResourceTarget : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            var c = (UnitContext)context;
            var unit = c.unit as WorkerUnit;
            if (unit == null)
            {
                // only worker units can have resource targets
                return;
            }

            unit.resourceTarget = null;
        }
    }
}