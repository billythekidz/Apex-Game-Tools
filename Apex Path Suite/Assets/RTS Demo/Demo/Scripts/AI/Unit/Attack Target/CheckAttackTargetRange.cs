namespace Apex.Demo.RTS.AI
{
    using Apex.AI;

    public sealed class CheckAttackTargetRange : UnitRangeContextualScorerBase
    {
        public override float Score(IAIContext context)
        {
            var c = (UnitContext)context;

            var attackTarget = c.attackTarget;
            if (attackTarget == null)
            {
                // unit has no attack target
                return 0f;
            }

            var distanceSqr = (attackTarget.position - c.unit.position).sqrMagnitude;
            if (IsComparisonTrue(c, distanceSqr))
            {
                return this.score;
            }

            return 0f;
        }
    }
}