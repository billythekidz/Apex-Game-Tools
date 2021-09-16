namespace Apex.Demo.RTS.AI
{
    using Apex.AI;

    public sealed class NullDefendTarget : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            ((UnitContext)context).defendTarget = null;
        }
    }
}