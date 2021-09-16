namespace Apex.Demo.RTS.AI
{
    using Apex.AI;

    public sealed class NullTempTarget : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            ((UnitContext)context).temporaryTarget = null;
        }
    }
}