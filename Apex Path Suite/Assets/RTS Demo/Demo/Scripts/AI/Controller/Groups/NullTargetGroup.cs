namespace Apex.Demo.RTS.AI
{
    using Apex.AI;

    public sealed class NullTargetGroup : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            ((ControllerContext)context).targetGroup = null;
        }
    }
}