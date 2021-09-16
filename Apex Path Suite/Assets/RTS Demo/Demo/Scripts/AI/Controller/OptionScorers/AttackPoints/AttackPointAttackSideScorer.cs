namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;

    public sealed class AttackPointAttackSideScorer : OptionScorerBase<AttackPoint>
    {
        [ApexSerialization(defaultValue = false), MemberDependency("nearestSide", false)]
        public bool leftSide;

        [ApexSerialization(defaultValue = true), MemberDependency("leftSide", false)]
        public bool nearestSide = true;

        [ApexSerialization]
        public float score = 100f;

        public override float Score(IAIContext context, AttackPoint option)
        {
            var c = (ControllerContext)context;
            var centerPos = c.center.position;
            var forward = (c.controller.predictedEnemyBasePosition - centerPos);
            var dir = (option.position - centerPos);
            var angleDir = Utils.GetAngleDirection(forward, dir);

            if (this.nearestSide)
            {
                // figure out which side the attack point resides on
                var attackTargetDir = (option.attackTarget.position - centerPos);
                var targetDir = Utils.GetAngleDirection(forward, attackTargetDir);
                this.leftSide = targetDir < 0f;
            }

            if (this.leftSide)
            {
                return angleDir < 0f ? this.score : 0f;
            }

            return angleDir > 0f ? this.score : 0f;
        }
    }
}