namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;
    using UnityEngine;

    public sealed class UnitPositionProximityToPredictedEnemyBasePositionScorer : OptionScorerBase<Vector3>
    {
        [ApexSerialization(defaultValue = 1f)]
        public float factor = 1f;

        [ApexSerialization]
        public float maxScore = 100f;

        public override float Score(IAIContext context, Vector3 option)
        {
            var c = (UnitContext)context;
            var distance = (option - c.unit.controller.predictedEnemyBasePosition).magnitude * this.factor;
            return Mathf.Max(0f, this.maxScore - distance);
        }
    }
}