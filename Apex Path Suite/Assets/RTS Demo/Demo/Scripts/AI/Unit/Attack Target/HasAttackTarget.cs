namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Apex.Serialization;

    public sealed class HasAttackTarget : ContextualScorerBase
    {
        [ApexSerialization(defaultValue = false), FriendlyName("Not", "Set to true to inverse the logic of this scorer, e.g. instead of scoring when true, it scores when false.")]
        public bool not;

        public override float Score(IAIContext context)
        {
            var c = (UnitContext)context;
            if (c.attackTarget != null)
            {
                return this.not ? 0f : this.score;
            }

            return this.not ? this.score : 0f;
        }
    }
}