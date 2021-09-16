namespace Apex.Demo.RTS.AI
{
    using Apex.AI;

    public sealed class SetCenterAsDefendTarget : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            var c = (UnitContext)context;
            c.defendTarget = c.unit.controller.center;
        }
    }
}