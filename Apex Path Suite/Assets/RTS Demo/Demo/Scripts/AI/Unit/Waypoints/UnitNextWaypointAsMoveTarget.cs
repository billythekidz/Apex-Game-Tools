namespace Apex.Demo.RTS.AI
{
    using Apex.AI;

    public sealed class UnitNextWaypointAsMoveTarget : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            var c = (UnitContext)context;
            if (c.waypoints.Count == 0)
            {
                return;
            }

            c.moveTarget = c.waypoints.Dequeue();
        }
    }
}