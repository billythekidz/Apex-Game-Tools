namespace Apex.Demo.RTS.AI
{
    using Apex.AI;

    public sealed class NullGuardTarget : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            ((UnitContext)context).guardTarget = null;
        }
    }
}