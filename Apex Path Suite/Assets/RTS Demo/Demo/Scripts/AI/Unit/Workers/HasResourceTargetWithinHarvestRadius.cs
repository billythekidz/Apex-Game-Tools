namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Apex.Serialization;

    public sealed class HasResourceTargetWithinHarvestRadius : ContextualScorerBase
    {
        [ApexSerialization(defaultValue = false), FriendlyName("Not", "Set to true to inverse the logic of this scorer, e.g. instead of scoring when true, it scores when false.")]
        public bool not;

        public override float Score(IAIContext context)
        {
            var c = (UnitContext)context;
            var unit = c.unit as WorkerUnit;
            if (unit == null)
            {
                return 0f;
            }

            var resourceTarget = unit.resourceTarget;
            if (resourceTarget == null)
            {
                return 0f;
            }

            var distance = (resourceTarget.position - unit.position).sqrMagnitude;
            if (distance < (unit.harvestRadius * unit.harvestRadius))
            {
                return this.not ? 0f : this.score;
            }

            return this.not ? this.score : 0f;
        }
    }
}