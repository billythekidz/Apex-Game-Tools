namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;
    using UnityEngine;

    public sealed class ControllerStructureProximityToPredictedEnemyBasePositionScorer : OptionScorerBase<Vector3>
    {
        [ApexSerialization(defaultValue = 1f)]
        public float factor = 1f;

        [ApexSerialization]
        public float maxScore = 1000f;

        public override float Score(IAIContext context, Vector3 option)
        {
            var c = (ControllerContext)context;
            var distance = (c.controller.predictedEnemyBasePosition - option).magnitude * this.factor;
            return Mathf.Max(0f, this.maxScore - distance);
        }
    }
}