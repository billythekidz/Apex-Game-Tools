namespace Apex.Demo.RTS.AI
{
    using Apex.AI;

    public sealed class CheckGuardTargetRange : UnitRangeContextualScorerBase
    {
        public override float Score(IAIContext context)
        {
            var c = (UnitContext)context;

            var guardTarget = c.guardTarget;
            if (!guardTarget.HasValue)
            {
                return 0f;
            }

            var distanceSqr = (guardTarget.Value - c.unit.position).sqrMagnitude;
            if (IsComparisonTrue(c, distanceSqr))
            {
                return this.score;
            }

            return 0f;
        }
    }
}