namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;
    using UnityEngine;

    public sealed class StagingPointLowestThreatScorer : OptionScorerBase<StagingPoint>
    {
        [ApexSerialization(defaultValue = 1f)]
        public float factor = 1f;

        [ApexSerialization]
        public float maxScore = 1000f;

        public override float Score(IAIContext context, StagingPoint option)
        {
            var threat = option.cell.threat * -1f; // by multiplying with -1, we score lowest threat (negative threat cells) higher
            return Mathf.Min(this.maxScore, threat);
        }
    }
}