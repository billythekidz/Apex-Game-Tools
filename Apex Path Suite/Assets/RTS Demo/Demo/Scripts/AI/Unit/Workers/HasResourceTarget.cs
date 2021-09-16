namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Apex.Serialization;

    public sealed class HasResourceTarget : ContextualScorerBase
    {
        [ApexSerialization(defaultValue = true)]
        public bool mustHaveResources = true;

        [ApexSerialization(defaultValue = false), FriendlyName("Not", "Set to true to inverse the logic of this scorer, e.g. instead of scoring when true, it scores when false.")]
        public bool not;

        public override float Score(IAIContext context)
        {
            var c = (UnitContext)context;
            var unit = c.unit as WorkerUnit;
            if (unit == null)
            {
                // only worker units can have resource targets
                return 0f;
            }

            var resourceTarget = unit.resourceTarget;
            if (resourceTarget != null && (!this.mustHaveResources || resourceTarget.currentResources > 0))
            {
                return this.not ? 0f : this.score;
            }

            return this.not ? this.score : 0f;
        }
    }
}