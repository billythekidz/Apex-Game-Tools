namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;

    public sealed class ControllerCheckTargetGroupRange : ContextualScorerBase
    {
        [ApexSerialization(defaultValue = CompareOperator.LessThan), FriendlyName("Comparison", "Select what kind of comparison to do")]
        public CompareOperator comparison = CompareOperator.LessThan;

        [ApexSerialization, MemberDependency("useStructureGridSize", false)]
        public float customRange = 10f;

        [ApexSerialization(defaultValue = true)]
        public bool useStructureGridSize = true;

        [ApexSerialization(defaultValue = false)]
        public bool relativeToEnemyBase;

        public override float Score(IAIContext context)
        {
            var c = (ControllerContext)context;
            var targetGroup = c.targetGroup;
            if (targetGroup == null)
            {
                return 0f;
            }

            var range = this.customRange;
            if (this.useStructureGridSize)
            {
                range = (c.controller.structureGrid.size * Constants.SquareRootTwo);
            }

            var pos = this.relativeToEnemyBase ? c.controller.predictedEnemyBasePosition : c.controller.position;
            var distanceSqr = (pos - targetGroup.centerOfGravity).sqrMagnitude;
            if (Utils.IsOperatorTrue(this.comparison, distanceSqr, (range * range)))
            {
                return this.score;
            }

            return 0f;
        }
    }
}