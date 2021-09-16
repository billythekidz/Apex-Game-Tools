namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Apex.Serialization;

    public sealed class MoveToCenterStructure : ActionBase
    {
        [ApexSerialization]
        public bool setMoveTarget;

        public override void Execute(IAIContext context)
        {
            var c = (UnitContext)context;
            var pos = c.unit.controller.center.GetHarvestReturnPosition(c.unit as WorkerUnit);
            c.unit.MoveTo(pos);

            if (this.setMoveTarget)
            {
                c.moveTarget = pos;
            }
        }
    }
}