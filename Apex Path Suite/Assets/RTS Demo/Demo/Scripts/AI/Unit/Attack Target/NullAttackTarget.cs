namespace Apex.Demo.RTS.AI
{
    using Apex.AI;

    public sealed class NullAttackTarget : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            ((UnitContext)context).attackTarget = null;
        }
    }
}