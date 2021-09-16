namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;
    using UnityEngine;

    public sealed class UnitTimeSinceLastAttacked : ContextualScorerBase
    {
        [ApexSerialization(defaultValue = 5f)]
        public float threshold = 5f;

        [ApexSerialization(defaultValue = false), FriendlyName("Not", "Set to true to inverse the logic of this scorer, e.g. instead of scoring when true, it scores when false.")]
        public bool not;

        public override float Score(IAIContext context)
        {
            var c = (UnitContext)context;
            var time = Time.timeSinceLevelLoad;
            if (time - c.unit.lastAttacked < this.threshold)
            {
                return this.not ? 0f : this.score;
            }

            return this.not ? this.score : 0f;
        }
    }
}