namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;

    public abstract class ControllerIssueGroupOrder_Base : ActionBase
    {
        [ApexSerialization(defaultValue = 1), MemberCategory(null, 0)]
        public int orderPriority = 1;

        protected void IssueOrderToGroup(IOrder order, UnitGroup group)
        {
            var count = group.count;
            for (int i = 0; i < count; i++)
            {
                group[i].orders.Add(order);
            }
        }
    }
}