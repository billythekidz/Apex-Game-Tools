namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;

    public sealed class UnitAttackTargetIsLastAttackerScorer : OptionScorerBase<IHasHealth>
    {
        [ApexSerialization(defaultValue = false), FriendlyName("Not", "Set to true to inverse the logic of this scorer, e.g. instead of scoring when true, it scores when false.")]
        public bool not;

        [ApexSerialization]
        public float score = 100f;

        public override float Score(IAIContext context, IHasHealth attackTarget)
        {
            var c = (UnitContext)context;
            if (c.unit.lastAttacker != null && c.unit.lastAttacker.id == attackTarget.id)
            {
                return this.not ? 0f : this.score;
            }

            return this.not ? this.score : 0f;
        }
    }
}