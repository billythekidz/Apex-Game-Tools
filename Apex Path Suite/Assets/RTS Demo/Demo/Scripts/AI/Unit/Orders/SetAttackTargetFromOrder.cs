namespace Apex.Demo.RTS.AI
{
    using Apex.AI;

    public sealed class SetAttackTargetFromOrder : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            var c = (UnitContext)context;
            var order = c.unit.currentOrder;
            if (order.orderCategory == OrderCategoryType.Offensive)
            {
                c.attackTarget = ((OffensiveOrderBase)order).target;
            }
        }
    }
}