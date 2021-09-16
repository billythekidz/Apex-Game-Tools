namespace Apex.Demo.RTS.AI
{
    using System.Collections.Generic;
    using Apex.AI;
    using Serialization;
    using Utilities;

    public sealed class ControllerIssueGroupOrder_DirectBaseAttack : ControllerIssueGroupOrder_Base
    {
        [ApexSerialization, MemberCategory("Attack Target", 10)]
        private List<IOptionScorer<IHasHealth>> _targetScorers = new List<IOptionScorer<IHasHealth>>();

        public override void Execute(IAIContext context)
        {
            var c = (ControllerContext)context;

            var group = c.targetGroup;
            if (group == null || group.count == 0)
            {
                return;
            }

            // find attack target. Get best from controller's enemy observations.
            var list = ListBufferPool.GetBuffer<IHasHealth>(c.controller.enemyObservations.Count);
            c.controller.GetObservedEnemies(list);
            var bestTarget = Utils.GetBestFromList(c, list, _targetScorers);
            ListBufferPool.ReturnBuffer(list);

            // Issue new order
            var order = new DirectBaseAttackOrder(this.orderPriority, bestTarget);
            IssueOrderToGroup(order, group);
        }
    }
}