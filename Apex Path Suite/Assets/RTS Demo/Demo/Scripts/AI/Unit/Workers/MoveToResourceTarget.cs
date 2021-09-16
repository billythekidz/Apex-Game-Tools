namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Apex.Serialization;

    public sealed class MoveToResourceTarget : ActionBase
    {
        [ApexSerialization]
        public bool setMoveTarget;

        public override void Execute(IAIContext context)
        {
            var c = (UnitContext)context;
            var unit = c.unit as WorkerUnit;
            if (unit == null)
            {
                // only worker units can have resource targets
                return;
            }

            var resourceTarget = unit.resourceTarget;
            if (resourceTarget == null)
            {
                // unit has no resource target
                return;
            }

            var pos = resourceTarget.GetHarvestPosition(unit);
            unit.MoveTo(pos);

            if (this.setMoveTarget)
            {
                c.moveTarget = pos;
            }
        }
    }
}