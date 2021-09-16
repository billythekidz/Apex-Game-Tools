namespace Apex.Demo.RTS.AI
{
    using System.Collections.Generic;
    using Apex.AI;
    using Serialization;
    using UnityEngine;
    using Utilities;

    public abstract class ControllerIssueGroupOrder_EngageBase : ControllerIssueGroupOrder_Base
    {
        [ApexSerialization, MemberCategory("Attack Target", 10)]
        protected List<IOptionScorer<IHasHealth>> _targetScorers = new List<IOptionScorer<IHasHealth>>();

        [ApexSerialization, MemberCategory("Attack Point", 20)]
        public float attackPointRadius = 40f;

        [ApexSerialization, MemberCategory("Attack Point", 20)]
        public float attackPointSamplingDistance = 10f;

        [ApexSerialization, MemberCategory("Attack Point", 20)]
        public float attackPointSamplingSpacing = 2f;

        [ApexSerialization, MemberCategory("Attack Point", 21)]
        protected List<IOptionScorer<AttackPoint>> _attackPointScorers = new List<IOptionScorer<AttackPoint>>();

        protected IHasHealth GetAttackTarget(ControllerContext context)
        {
            // find attack target. Get best from controller's enemy observations.
            var list = ListBufferPool.GetBuffer<IHasHealth>(context.controller.enemyObservations.Count);
            context.controller.GetObservedEnemies(list);
            var bestTarget = Utils.GetBestFromList(context, list, _targetScorers);
            ListBufferPool.ReturnBuffer(list);

            return bestTarget;
        }

        protected AttackPoint GetAttackPoint(ControllerContext context, Vector3 center, IHasHealth attackTarget)
        {
            var steps = Mathf.CeilToInt(((this.attackPointSamplingDistance * 2f) / this.attackPointSamplingDistance));
            var list = ListBufferPool.GetBuffer<AttackPoint>(steps * steps);

            var mapGrid = context.controller.mapGrid;

            for (int x = 0; x < steps; x++)
            {
                for (int z = 0; z < steps; z++)
                {
                    var pos = center + new Vector3(
                        -this.attackPointSamplingDistance + (x * this.attackPointSamplingSpacing),
                        0f,
                        -this.attackPointSamplingDistance + (z * this.attackPointSamplingSpacing));

                    list.Add(new AttackPoint(mapGrid.GetNearestCell(pos), attackTarget));
                }
            }

            var attackPoint = Utils.GetBestFromList(context, list, _attackPointScorers);
            ListBufferPool.ReturnBuffer(list);

            return attackPoint;
        }
    }
}