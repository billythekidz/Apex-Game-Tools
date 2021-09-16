namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;
    using UnityEngine;

    public sealed class ControllerGroupProximityToPredictedEnemyBaseScorer : OptionScorerBase<UnitGroup>
    {
        [ApexSerialization(defaultValue = 1f)]
        public float factor = 1f;

        [ApexSerialization]
        public float maxScore = 100f;

        public override float Score(IAIContext context, UnitGroup group)
        {
            var c = (ControllerContext)context;
            var distance = (c.controller.predictedEnemyBasePosition - group.centerOfGravity).magnitude * this.factor;
            return Mathf.Max(0f, this.maxScore - distance);
        }
    }
}