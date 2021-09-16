namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using UnityEngine;

    public sealed class ControllerIssueGroupOrder_FlankingAttack : ControllerIssueGroupOrder_EngageBase
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

            // find attack points. Compute fixed points and do sampling around
            var enemyBasePos = c.controller.predictedEnemyBasePosition;
            var baseToBaseDir = (c.controller.position - enemyBasePos);
            var baseToBaseOrtho = new Vector3(-baseToBaseDir.z, baseToBaseDir.y, baseToBaseDir.x).normalized * this.attackPointRadius; // get orthogonal vector

            // compute points
            var negativeAttackPoint = GetAttackPoint(c, enemyBasePos - baseToBaseOrtho, bestTarget);
            var positiveAttackPoint = GetAttackPoint(c, enemyBasePos + baseToBaseOrtho, bestTarget);

            // Issue new orders => split group in two halves and assign order to each of them
            var splitGroup = group.Split(0, Mathf.CeilToInt(group.count * 0.5f) - 1);
            if (splitGroup != null)
            {
                var negOrder = new FlankingAttackOrder(this.orderPriority, bestTarget, negativeAttackPoint.position);
                IssueOrderToGroup(negOrder, splitGroup);
            }

            var posOrder = new FlankingAttackOrder(this.orderPriority, bestTarget, positiveAttackPoint.position);
            IssueOrderToGroup(posOrder, group);
        }
    }
}