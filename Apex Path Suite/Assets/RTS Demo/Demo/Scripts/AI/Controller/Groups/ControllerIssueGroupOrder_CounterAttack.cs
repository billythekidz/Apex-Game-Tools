namespace Apex.Demo.RTS.AI
{
    using System.Collections.Generic;
    using Apex.AI;
    using Serialization;
    using Utilities;

    public sealed class ControllerIssueGroupOrder_CounterAttack : ControllerIssueGroupOrder_Base
    {
        [ApexSerialization, MemberCategory("Attack Target", 5)]
        private List<IOptionScorer<IUnit>> _unitScorers = new List<IOptionScorer<IUnit>>();

        public override void Execute(IAIContext context)
        {
            var c = (ControllerContext)context;

            var group = c.targetGroup;
            if (group == null)
            {
                return;
            }

            var units = ListBufferPool.GetBuffer<IUnit>(c.controller.enemyObservations.Count);
            c.controller.GetObservedEnemies(units);

            var bestUnit = Utils.GetBestFromList(c, units, _unitScorers);
            ListBufferPool.ReturnBuffer(units);

            if (bestUnit == null)
            {
                return;
            }

            var order = new CounterAttackOrder(this.orderPriority, bestUnit);
            IssueOrderToGroup(order, group);
        }
    }
}