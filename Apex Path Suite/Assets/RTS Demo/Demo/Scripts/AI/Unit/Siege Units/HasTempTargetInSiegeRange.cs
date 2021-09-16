namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;

    public sealed class HasTempTargetInSiegeRange : ContextualScorerBase
    {
        [ApexSerialization(defaultValue = false), FriendlyName("Not", "Set to true to inverse the logic of this scorer, e.g. instead of scoring when true, it scores when false.")]
        public bool not;

        public override float Score(IAIContext context)
        {
            var c = (UnitContext)context;
            var unit = c.unit as SiegeUnit;
            if (unit == null)
            {
                // only siege units have a specific range band
                return 0f;
            }

            var tempTarget = c.temporaryTarget;
            if (tempTarget == null)
            {
                // unit has no attack target
                return 0f;
            }

            var distanceSqr = (tempTarget.position - unit.position).sqrMagnitude;
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