namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;

    public sealed class GroupIdleUnitCountScorer : OptionScorerBase<UnitGroup>
    {
        [ApexSerialization]
        public float scorePerIdleUnit = 100f;

        public override float Score(IAIContext context, UnitGroup group)
        {
            var score = 0f;
            var count = group.count;
            for (int i = 0; i < count; i++)
            {
                if (group[i].isIdle)
                {
                    score += this.scorePerIdleUnit;
                }
            }

            return score;
        }
    }
}