namespace Apex.Demo.RTS.AI
{
    using Apex.AI;

    public sealed class NullMoveTarget : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            ((UnitContext)context).moveTarget = null;
        }
    }
}