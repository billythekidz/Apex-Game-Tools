namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;
    using UnityEngine;

    public sealed class ControllerSplitGroup : ActionWithOptions<UnitGroup>
    {
        [ApexSerialization(defaultValue = false)]
        public bool useDefaultGroup;

        [ApexSerialization(defaultValue = 1f), FriendlyName("Split Percentage", "The percentage of the groups' units that are moved to the new group. 1 = 100%")]
        public float _splitPercentage = 1f;

        [ApexSerialization(defaultValue = false)]
        public bool setNewGroupAsTargetGroup;

        public override void Execute(IAIContext context)
        {
            var c = (ControllerContext)context;
            var group = this.useDefaultGroup ? c.controller.defaultGroup : this.GetBest(c, c.controller.groups);

            // split entire group if split percentage is at 1
            var newGroup = _splitPercentage == 1f ? group.Split() : group.Split(0, Mathf.RoundToInt(group.count * _splitPercentage));
            if (newGroup == null)
            {
                // no units to form a group of
                return;
            }

            c.controller.groups.Add(newGroup);

            if (this.setNewGroupAsTargetGroup)
            {
                c.targetGroup = newGroup;
            }
        }
    }
}