namespace Apex.Demo.RTS.AI
{
    using System.Collections.Generic;
    using System.Linq;
    using Apex.AI;
    using Serialization;
    using UnityEngine;
    using Utilities;

    public sealed class ControllerIssueGroupOrder_WeakestPointAttack : ControllerIssueGroupOrder_EngageBase
    {
        [ApexSerialization, MemberCategory("Attack Point", 22)]
        public float attackPointCenterRangeFromAttackTarget = 100f;

        [ApexSerialization, MemberCategory("Attack Point", 23)]
        private List<IOptionScorer<AttackPoint>> _attackPointCenterScorers = new List<IOptionScorer<AttackPoint>>();

        [ApexSerialization(defaultValue = 1), MemberCategory("Staging Points", 30)]
        public int stagingPointCount = 1;

        [ApexSerialization, MemberCategory("Staging Points", 31)]
        private List<IOptionScorer<StagingPoint>> _stagingPointScorers = new List<IOptionScorer<StagingPoint>>();

        public override void Execute(IAIContext context)
        {
            var c = (ControllerContext)context;

            var targetGroup = c.targetGroup;
            if (targetGroup == null || targetGroup.count == 0)
            {
                return;
            }

            var attackTarget = GetAttackTarget(c);
            if (attackTarget == null)
            {
                return;
            }

            var attackPointRangeSqr = this.attackPointCenterRangeFromAttackTarget * this.attackPointCenterRangeFromAttackTarget;

            // only evaluate map grid cells that are within range of the attack target
            var mapGridCells = c.controller.mapGrid.cells;
            var mapGridList = ListBufferPool.GetBuffer<AttackPoint>(mapGridCells.Length);
            for (int i = 0; i < mapGridCells.Length; i++)
            {
                var distanceSqr = (mapGridCells[i].center - attackTarget.position).sqrMagnitude;
                if (distanceSqr > attackPointRangeSqr)
                {
                    continue;
                }

                mapGridList.Add(new AttackPoint(mapGridCells[i], attackTarget));
            }

            var attackPointTemp = Utils.GetBestFromList(c, mapGridList, _attackPointCenterScorers);
            ListBufferPool.ReturnBuffer(mapGridList);

            // find the actual attack point by sampling around the attack point cell
            var attackPoint = GetAttackPoint(c, attackPointTemp.position, attackTarget);

            List<Vector3> stagingPoints = null;
            if (this.stagingPointCount > 0)
            {
                // prepare the special staging point options
                var stagingPointOptions = ListBufferPool.GetBuffer<StagingPoint>(mapGridCells.Length);
                for (int i = 0; i < mapGridCells.Length; i++)
                {
                    var distanceSqr = (mapGridCells[i].center - attackPoint.position).sqrMagnitude;
                    if (distanceSqr > attackPointRangeSqr)
                    {
                        continue;
                    }

                    stagingPointOptions.Add(new StagingPoint(mapGridCells[i], attackPoint.position, attackTarget));
                }

#if UNITY_EDITOR
                // TODO: REMOVEME: TOTALLY ONLY FOR DEBUG PURPOSES; REMOVE BEFORE RELEASE!
                var allScores = ListBufferPool.GetBuffer<ScoredOption<StagingPoint>>(mapGridCells.Length);
                Utils.GetBestSubsetFromList(c, stagingPointOptions, mapGridCells.Length, _stagingPointScorers, allScores);
                Editor.StagingPointScoreVisualizer.AddScores(allScores);
                ListBufferPool.ReturnBuffer(allScores);

#endif

                // get the scored staging points
                var stagingPointsTemp = ListBufferPool.GetBuffer<ScoredOption<StagingPoint>>(this.stagingPointCount);
                Utils.GetBestSubsetFromList(c, stagingPointOptions, this.stagingPointCount, _stagingPointScorers, stagingPointsTemp);

                // ultimately only select the position of the staging points
                stagingPoints = stagingPointsTemp.Count > 1 ?
                    new List<Vector3>(stagingPointsTemp.Select(p => p.option.position)) :
                    new List<Vector3> { stagingPointsTemp[0].option.position };

                // return lists
                ListBufferPool.ReturnBuffer(stagingPointOptions);
                ListBufferPool.ReturnBuffer(stagingPointsTemp);
            }

            var order = new WeakestPointAttackOrder(this.orderPriority, attackTarget, attackPoint.position, stagingPoints);
            IssueOrderToGroup(order, targetGroup);
        }
    }
}