namespace Apex.Demo.RTS.AI
{
    using System.Collections.Generic;
    using Apex.AI;
    using Serialization;

    public sealed class ControllerMergeWithGroup : ActionWithOptions<UnitGroup>
    {
        [ApexSerialization(defaultValue = true)]
        public bool useDefaultGroup = true;

        [ApexSerialization, MemberDependency("useDefaultGroup", false)]
        private List<IOptionScorer<UnitGroup>> _mergeFromGroupScorers = new List<IOptionScorer<UnitGroup>>();

        public override void Execute(IAIContext context)
        {
            var c = (ControllerContext)context;
            var group = this.GetBest(c, c.controller.groups);
            if (group == null)
            {
                return;
            }

            var otherGroup = this.useDefaultGroup ? c.controller.defaultGroup : Utils.GetBestFromList(c, c.controller.groups, _mergeFromGroupScorers);
            if (otherGroup == null)
            {
                return;
            }

            if (ReferenceEquals(group, otherGroup))
            {
                // cannot merge group with itself
                return;
            }

            group.Merge(otherGroup);

            if (!this.useDefaultGroup && otherGroup != c.controller.defaultGroup)
            {
                // never remove the default group
                c.controller.groups.Remove(otherGroup);
            }
        }
    }
}