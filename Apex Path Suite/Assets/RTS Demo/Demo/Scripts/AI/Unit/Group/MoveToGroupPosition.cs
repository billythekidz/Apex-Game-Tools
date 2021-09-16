namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;

    public sealed class MoveToGroupPosition : ActionBase
    {
        [ApexSerialization(defaultValue = false)]
        public bool setMoveTarget;

        public override void Execute(IAIContext context)
        {
            var c = (UnitContext)context;
            var group = c.unit.group;
            if (group == null)
            {
                return;
            }

            var pos = group.position;
            if (this.setMoveTarget)
            {
                c.moveTarget = pos;
            }

            c.unit.MoveTo(pos);
        }
    }
}