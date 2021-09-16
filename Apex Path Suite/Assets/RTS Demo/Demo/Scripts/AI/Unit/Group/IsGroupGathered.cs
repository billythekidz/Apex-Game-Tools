namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;

    public sealed class IsGroupGathered : ContextualScorerBase
    {
        [ApexSerialization]
        public float groupDistanceThreshold = 5f;

        [ApexSerialization(defaultValue = false), FriendlyName("Not", "Set to true to inverse the logic of this scorer, e.g. instead of scoring when true, it scores when false.")]
        public bool not;

        public override float Score(IAIContext context)
        {
            var c = (UnitContext)context;

            var group = c.unit.group;
            var cog = group.centerOfGravity;
            var count = group.count;
            for (int i = 0; i < count; i++)
            {
                var distanceSqr = (group[i].position - cog).sqrMagnitude;
                if (distanceSqr > (this.groupDistanceThreshold * this.groupDistanceThreshold))
                {
                    // at least one unit is out of range
                    return this.not ? this.score : 0f;
                }
            }

            return this.not ? 0f : this.score;
        }
    }
}