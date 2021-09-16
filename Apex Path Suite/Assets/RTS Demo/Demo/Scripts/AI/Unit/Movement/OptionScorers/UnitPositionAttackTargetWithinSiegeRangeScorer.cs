namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;
    using UnityEngine;

    public sealed class UnitPositionAttackTargetWithinSiegeRangeScorer : OptionScorerBase<Vector3>
    {
        [ApexSerialization]
        public float score = 1000f;

        [ApexSerialization(defaultValue = false), FriendlyName("Not", "Set to true to inverse the logic of this scorer, e.g. instead of scoring when true, it scores when false.")]
        public bool not;

        public override float Score(IAIContext context, Vector3 position)
        {
            var c = (UnitContext)context;
            var unit = c.unit as SiegeUnit;
            if (unit == null)
            {
                // only siege units have a specific range band
                return 0f;
            }

            var attackTarget = c.attackTarget;
            if (attackTarget == null)
            {
                return 0f;
            }

            var distanceSqr = (attackTarget.position - unit.position).sqrMagnitude;
            var maxRadiusSqr = unit.attackRadius * unit.attackRadius;
            var minRadiusSqr = unit.minimumAttackRadius * unit.minimumAttackRadius;
            if (distanceSqr >= minRadiusSqr && distanceSqr <= maxRadiusSqr)
            {
                return this.not ? 0f : this.score;
            }

            return this.not ? this.score : 0f;
        }
    }
}