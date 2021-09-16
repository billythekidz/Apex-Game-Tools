namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Apex.Serialization;

    public sealed class UnitTypeScorer : OptionScorerBase<IUnit>
    {
        [ApexSerialization]
        public UnitType unitType;

        [ApexSerialization]
        public float score;

        [ApexSerialization(defaultValue = false), FriendlyName("Not", "Set to true to inverse the logic of this scorer, e.g. instead of scoring when true, it scores when false.")]
        public bool not;

        public override float Score(IAIContext context, IUnit option)
        {
            if (option.unitType == this.unitType)
            {
                return this.not ? 0f : this.score;
            }

            return this.not ? this.score : 0f;
        }
    }
}