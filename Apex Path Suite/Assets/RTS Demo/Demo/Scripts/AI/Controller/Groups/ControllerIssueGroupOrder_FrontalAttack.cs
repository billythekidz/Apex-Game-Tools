namespace Apex.Demo.RTS.AI
{
    using Apex.AI;

    public sealed class ControllerIssueGroupOrder_FrontalAttack : ControllerIssueGroupOrder_EngageBase
    {
        public override void Execute(IAIContext context)
        {
            var c = (ControllerContext)context;

            var group = c.targetGroup;
            if (group == null || group.count == 0)
            {
                return;
            }

            // find attack target. Get best from controller's enemy observations.
            var bestTarget = GetAttackTarget(c);
            if (bestTarget == null)
            {
                return;
            }

            // get a position on a straight line between "our" base and the enemy's
            var enemyBasePos = c.controller.predictedEnemyBasePosition;
            var attackPointCenter = enemyBasePos + (c.controller.position - enemyBasePos).normalized * this.attackPointRadius;
            var attackPoint = GetAttackPoint(c, attackPointCenter, bestTarget);

            // Issue new order
            var order = new FrontalAttackOrder(this.orderPriority, bestTarget, attackPoint.position);
            IssueOrderToGroup(order, group);
        }
    }
}