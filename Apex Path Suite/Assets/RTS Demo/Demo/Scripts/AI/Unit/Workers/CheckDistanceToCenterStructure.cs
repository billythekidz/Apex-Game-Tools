namespace Apex.Demo.RTS.AI
{
    using Apex.AI;

    public sealed class CheckDistanceToCenterStructure : UnitRangeContextualScorerBase
    {
        public override float Score(IAIContext context)
        {
            var c = (UnitContext)context;
            var center = c.unit.controller.center;
            var distance = (c.unit.position - center.position).sqrMagnitude;
            if (IsComparisonTrue(c, distance))
            {
                return this.score;
            }

            return 0f;
        }
    }
}