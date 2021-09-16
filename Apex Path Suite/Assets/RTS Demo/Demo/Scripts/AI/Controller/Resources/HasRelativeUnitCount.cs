namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;

    public class HasRelativeUnitCount : ContextualScorerBase
    {
        [ApexSerialization]
        public UnitType unitType;

        [ApexSerialization]
        public UnitType relativeTo;

        [ApexSerialization]
        public CompareOperator comparison = CompareOperator.LessThan;

        [ApexSerialization]
        public float threshold = 1f;

        public override float Score(IAIContext context)
        {
            var c = (ControllerContext)context;
            var unitCount = c.controller.GetUnitCount(this.unitType);
            var relativeCount = c.controller.GetUnitCount(this.relativeTo);
            var factor = relativeCount > 0 ? unitCount / relativeCount : unitCount;
            if (Utils.IsOperatorTrue(this.comparison, factor, this.threshold))
            {
                return this.score;
            }

            return 0f;
        }
    }
}