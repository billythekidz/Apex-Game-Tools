namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;
    using UnityEngine;

    public sealed class HasGroupUnitCount : ContextualScorerBase
    {
        [ApexSerialization(defaultValue = CompareOperator.GreaterThan)]
        public CompareOperator comparison = CompareOperator.GreaterThan;

        [ApexSerialization]
        public int threshold = 10;

        [ApexSerialization(defaultValue = false)]
        public bool useDefaultGroup;

        [ApexSerialization(defaultValue = 0), FriendlyName("Group Index", "The index of the group to check. Note that group 0 is the default group."), MemberDependency("useDefaultGroup", false)]
        public int groupIndex;

        public override float Score(IAIContext context)
        {
            var c = (ControllerContext)context;
            var groups = c.controller.groups;
            if (this.groupIndex < 0 || this.groupIndex >= groups.Count)
            {
                // out of bounds
                Debug.LogWarning(this.ToString() + " => OUT OF BOUNDS: Cannot use group index: " + this.groupIndex + ", because the groups count is " + groups.Count);
                return 0f;
            }

            if (Utils.IsOperatorTrue(this.comparison, groups[this.groupIndex].count, this.threshold))
            {
                return this.score;
            }

            return 0f;
        }
    }
}