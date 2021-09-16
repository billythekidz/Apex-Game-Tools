namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;
    using UnityEngine;

    public sealed class AttackPointProximityToPredictedEnemyBaseScorer : OptionScorerBase<AttackPoint>
    {
        [ApexSerialization(defaultValue = 1f)]
        public float factor = 1f;

        [ApexSerialization]
        public float maxScore = 1000f;

        public override float Score(IAIContext context, AttackPoint option)
        {
            var c = (ControllerContext)context;
            var distance = (c.controller.predictedEnemyBasePosition - option.position).magnitude * this.factor;
            return Mathf.Max(0f, this.maxScore - distance);
        }
    }
}