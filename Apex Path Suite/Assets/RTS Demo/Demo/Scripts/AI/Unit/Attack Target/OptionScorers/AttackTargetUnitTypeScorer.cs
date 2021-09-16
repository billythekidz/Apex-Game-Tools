namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;

    public sealed class AttackTargetUnitTypeScorer : OptionScorerBase<IHasHealth>
    {
        [ApexSerialization]
        public UnitType unitType = UnitType.Any;

        [ApexSerialization]
        public float score = 10f;

        [ApexSerialization(defaultValue = false), FriendlyName("Not", "Set to true to inverse the logic of this scorer, e.g. instead of scoring when true, it scores when false.")]
        public bool not;

        public override float Score(IAIContext context, IHasHealth option)
        {
            var unit = option as IUnit;
            if (unit == null)
            {
                return 0f;
            }

            if (this.unitType == UnitType.Any || unit.unitType == this.unitType)
            {
                return this.not ? 0f : this.score;
            }

            return this.not ? this.score : 0f;
        }
    }
}