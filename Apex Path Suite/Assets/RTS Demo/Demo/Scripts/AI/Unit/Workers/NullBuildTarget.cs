namespace Apex.Demo.RTS.AI
{
    using Apex.AI;

    public sealed class NullBuildTarget : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            ((UnitContext)context).buildTarget = null;
        }
    }
}