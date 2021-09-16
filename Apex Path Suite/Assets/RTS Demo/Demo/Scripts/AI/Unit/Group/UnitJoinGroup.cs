namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;
    using UnityEngine;

    public sealed class UnitJoinGroup : ActionBase
    {
        [ApexSerialization, MemberDependency("useDefaultGroup", false)]
        public int groupIndex = 0;

        [ApexSerialization(defaultValue = false)]
        public bool useDefaultGroup;

        public override void Execute(IAIContext context)
        {
            var c = (UnitContext)context;
            var unit = c.unit;
            var controller = unit.controller;

            UnitGroup group;
            if (this.useDefaultGroup)
            {
                group = controller.defaultGroup;
            }
            else
            {
                if (this.groupIndex < 0 || this.groupIndex >= controller.groups.Count)
                {
                    Debug.LogError(this.ToString() + " could not join non-existing group at index == " + this.groupIndex.ToString());
                    return;
                }

                group = controller.groups[this.groupIndex];
            }

            if (unit.group != null)
            {
                unit.group.Remove(unit);
            }

            group.Add(unit);
        }
    }
}