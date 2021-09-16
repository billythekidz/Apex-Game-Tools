namespace Apex.Demo.RTS.AI
{
    using Apex.AI;

    public sealed class CheckTempTargetRange : UnitRangeContextualScorerBase
    {
        public override float Score(IAIContext context)
        {
            var c = (UnitContext)context;

            var tempTarget = c.temporaryTarget;
            if (tempTarget == null)
            {
                // unit has no attack target
                return 0f;
            }

            var distanceSqr = (tempTarget.position - c.unit.position).sqrMagnitude;
            if (IsComparisonTrue(c, distanceSqr))
            {
                return this.score;
            }

            return 0f;
        }
    }
}