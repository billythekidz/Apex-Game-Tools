namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;

    public sealed class AttackTargetStructureTypeScorer : OptionScorerBase<IHasHealth>
    {
        [ApexSerialization]
        public StructureType structureType = StructureType.Any;

        [ApexSerialization]
        public float score = 10f;

        [ApexSerialization(defaultValue = false), FriendlyName("Not", "Set to true to inverse the logic of this scorer, e.g. instead of scoring when true, it scores when false.")]
        public bool not;

        public override float Score(IAIContext context, IHasHealth option)
        {
            var structure = option as IStructure;
            if (structure == null)
            {
                return 0f;
            }

            if (this.structureType == StructureType.Any || structure.structureType == this.structureType)
            {
                return this.not ? 0f : this.score;
            }

            return this.not ? this.score : 0f;
        }
    }
}