namespace Apex.Demo.RTS.AI
{
    using Apex.AI;

    public sealed class SetBestTargetGroup : ActionWithOptions<UnitGroup>
    {
        public override void Execute(IAIContext context)
        {
            var c = (ControllerContext)context;
            var best = this.GetBest(c, c.controller.groups);
            if (best != null)
            {
                c.targetGroup = best;
            }
        }
    }
}