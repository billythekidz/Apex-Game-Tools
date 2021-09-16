namespace Apex.Demo.RTS.AI
{
    using Apex.AI;

    public sealed class CheckDefendTargetRange : UnitRangeContextualScorerBase
    {
        public override float Score(IAIContext context)
        {
            var c = (UnitContext)context;

            var defendTarget = c.defendTarget;
            if (defendTarget == null)
            {
                return 0f;
            }

            var distanceSqr = (defendTarget.position - c.unit.position).sqrMagnitude;
            if (IsComparisonTrue(c, distanceSqr))
            {
                return this.score;
            }

            return 0f;
        }
    }
}