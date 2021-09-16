namespace Apex.Demo.RTS.AI
{
    using Apex.AI;

    public sealed class CheckMoveTargetRange : UnitRangeContextualScorerBase
    {
        public override float Score(IAIContext context)
        {
            var c = (UnitContext)context;

            var moveTarget = c.moveTarget;
            if (!moveTarget.HasValue)
            {
                // unit has no move target
                return 0f;
            }

            var distanceSqr = (moveTarget.Value - c.unit.position).sqrMagnitude;
            if (IsComparisonTrue(c, distanceSqr))
            {
                return this.score;
            }

            return 0f;
        }
    }
}