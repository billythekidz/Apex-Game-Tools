namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Apex.Serialization;

    public sealed class StructureCheckRangeToAttackTarget : ContextualScorerBase
    {
        [ApexSerialization, FriendlyName("Comparison", "Select what kind of comparison to do")]
        public CompareOperator comparison = CompareOperator.LessThan;

        [ApexSerialization, MemberDependency("useScanRadius", false), MemberDependency("useAttackRadius", false), FriendlyName("Range", "A custom range to use for the evaluation")]
        public float range = 10f;

        [ApexSerialization, MemberDependency("useAttackRadius", false), FriendlyName("Use Scan Radius", "Set to true to use the structure's scanRadius as the range (Only supported for towers with scanning)")]
        public bool useScanRadius;

        [ApexSerialization, MemberDependency("useScanRadius", false), FriendlyName("Use Attack Radius", "Set to true to use the structure's attackRadius as the range (Only supported for Cannon Towers)")]
        public bool useAttackRadius;

        public override float Score(IAIContext context)
        {
            var c = (StructureContext)context;

            var attackTarget = c.attackTarget;
            if (attackTarget == null)
            {
                return 0f;
            }

            var radius = this.range;
            if (this.useScanRadius)
            {
                var scanRadius = ((IHasScanner)c.structure).scanRadius;
                radius = scanRadius;
            }
            else if (this.useAttackRadius)
            {
                var attackRadius = ((CannonTower)c.structure).attackRadius;
                radius = attackRadius;
            }

            // square it
            var radiusSqr = radius * radius;
            var distanceSqr = (attackTarget.position - c.structure.position).sqrMagnitude;
            if (Utils.IsOperatorTrue(this.comparison, distanceSqr, radiusSqr))
            {
                return this.score;
            }

            return 0f;
        }
    }
}