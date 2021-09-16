namespace Apex.Demo.RTS.AI
{
    using Apex.AI;

    public sealed class CheckGroupPositionRange : UnitRangeContextualScorerBase
    {
        public override float Score(IAIContext context)
        {
            var c = (UnitContext)context;
            var group = c.unit.group;
            if (group == null)
            {
                return 0f;
            }

            var distanceSqr = (c.unit.position - group.position).sqrMagnitude;
            if (IsComparisonTrue(c, distanceSqr))
            {
                return this.score;
            }

            return 0f;
        }
    }
}