namespace Apex.Demo.RTS.AI
{
    using Apex.AI;

    public sealed class MoveToMoveTarget : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            var c = (UnitContext)context;

            var moveTarget = c.moveTarget;
            if (!moveTarget.HasValue)
            {
                // unit has no move target
                return;
            }

            c.unit.MoveTo(moveTarget.Value);
        }
    }
}