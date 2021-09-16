namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;

    public sealed class HasTargetGroupIdleUnits : ContextualScorerBase
    {
        [ApexSerialization(defaultValue = CompareOperator.GreaterThanOrEquals)]
        public CompareOperator comparison = CompareOperator.GreaterThanOrEquals;

        [ApexSerialization, MemberDependency("allUnits", false)]
        public int threshold = 1;

        [ApexSerialization(defaultValue = false)]
        public bool allUnits;

        public override float Score(IAIContext context)
        {
            var c = (ControllerContext)context;
            var targetGroup = c.targetGroup;
            if (targetGroup == null)
            {
                return 0f;
            }

            var idleCount = 0;
            var count = targetGroup.count;
            for (int i = 0; i < count; i++)
            {
                if (targetGroup[i].isIdle)
                {
                    idleCount++;
                }
            }

            var threshold = this.allUnits ? targetGroup.count : this.threshold;
            if (Utils.IsOperatorTrue(this.comparison, idleCount, threshold))
            {
                return this.score;
            }

            return 0f;
        }
    }
}