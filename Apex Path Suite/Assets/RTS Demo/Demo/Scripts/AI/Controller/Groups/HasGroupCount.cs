namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;

    public sealed class HasGroupCount : ContextualScorerBase
    {
        [ApexSerialization]
        public CompareOperator comparison;

        [ApexSerialization]
        public int threshold;

        [ApexSerialization(defaultValue = false)]
        public bool ignoreDefaultGroup;

        public override float Score(IAIContext context)
        {
            var c = (ControllerContext)context;
            var count = c.controller.groups.Count;
            if (this.ignoreDefaultGroup)
            {
                count--;
            }

            if (Utils.IsOperatorTrue(this.comparison, count, this.threshold))
            {
                return this.score;
            }

            return 0f;
        }
    }
}