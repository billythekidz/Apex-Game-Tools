namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Apex.Serialization;

    public abstract class UnitRangeContextualScorerBase : ContextualScorerBase
    {
        [ApexSerialization(defaultValue = CompareOperator.LessThan), FriendlyName("Comparison", "Select what kind of comparison to do")]
        public CompareOperator comparison = CompareOperator.LessThan;

        [ApexSerialization(defaultValue = false)]
        public bool discardRange;

        [ApexSerialization, MemberDependency("discardRange", false), MemberDependency("useScanRadius", false), MemberDependency("useAttackRadius", false), FriendlyName("Range", "A custom range to use for the evaluation")]
        public float range = 10f;

        [ApexSerialization(defaultValue = false), MemberDependency("discardRange", false), MemberDependency("useAttackRadius", false), FriendlyName("Use Scan Radius", "Set to true to use the unit's scanRadius as the range")]
        public bool useScanRadius;

        [ApexSerialization(defaultValue = false), MemberDependency("discardRange", false), MemberDependency("useScanRadius", false), FriendlyName("Use Attack Radius", "Set to true to use the unit's attackRadius as the range")]
        public bool useAttackRadius;

        protected float GetRadiusSqr(UnitContext c)
        {
            var radius = this.range;
            if (this.useScanRadius)
            {
                radius = c.unit.scanRadius;
            }
            else if (this.useAttackRadius)
            {
                radius = c.unit.attackRadius;
            }

            return radius * radius;
        }

        protected bool IsComparisonTrue(UnitContext c, float valueSqr)
        {
            if (this.comparison == CompareOperator.None || this.discardRange)
            {
                return true;
            }

            var radiusSqr = GetRadiusSqr(c);
            return Utils.IsOperatorTrue(this.comparison, valueSqr, radiusSqr);
        }
    }
}