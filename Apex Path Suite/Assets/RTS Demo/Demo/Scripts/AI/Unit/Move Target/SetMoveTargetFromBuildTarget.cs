namespace Apex.Demo.RTS.AI
{
    using Apex.AI;

    public sealed class SetMoveTargetFromBuildTarget : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            var c = (UnitContext)context;
            var buildTarget = c.buildTarget;
            if (buildTarget == null)
            {
                return;
            }

            c.moveTarget = buildTarget.position;
        }
    }
}