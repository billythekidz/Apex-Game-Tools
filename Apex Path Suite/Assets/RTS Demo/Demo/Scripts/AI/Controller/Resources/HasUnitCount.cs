namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Apex.Serialization;

    public sealed class HasUnitCount : ContextualScorerBase
    {
        [ApexSerialization]
        public UnitType unitType = UnitType.Any;

        [ApexSerialization]
        public CompareOperator comparison = CompareOperator.GreaterThan;

        [ApexSerialization]
        public int threshold = 10;

        public override float Score(IAIContext context)
        {
            if (this.unitType == UnitType.None)
            {
                return 0f;
            }

            var c = (ControllerContext)context;
            var count = c.controller.GetUnitCount(this.unitType);
            if (Utils.IsOperatorTrue(this.comparison, count, threshold))
            {
                return this.score;
            }

            return 0f;
        }
    }
}