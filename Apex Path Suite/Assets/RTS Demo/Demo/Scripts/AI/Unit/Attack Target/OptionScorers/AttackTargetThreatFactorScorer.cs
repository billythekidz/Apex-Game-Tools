namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;
    using UnityEngine;

    public sealed class AttackTargetThreatFactorScorer : OptionScorerBase<IHasHealth>
    {
        [ApexSerialization(defaultValue = 1f)]
        public float factor = 1f;

        [ApexSerialization]
        public float maxScore = 100f;

        public override float Score(IAIContext context, IHasHealth option)
        {
            var threat = option.threatFactor * this.factor;
            return Mathf.Min(this.maxScore, threat);
        }
    }
}