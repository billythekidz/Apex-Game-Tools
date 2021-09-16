namespace Apex.Demo.RTS.AI
{
    using Apex.AI;

    public sealed class ControllerIssueGroupOrder_RetreatToBase : ControllerIssueGroupOrder_Base
    {
        public override void Execute(IAIContext context)
        {
            var c = (ControllerContext)context;
            var group = c.targetGroup;
            if (group == null)
            {
                return;
            }

            var order = new RetreatToBaseOrder(this.orderPriority);
            IssueOrderToGroup(order, group);
        }
    }
}