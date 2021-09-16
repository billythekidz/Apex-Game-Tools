namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;

    public sealed class MoveToTempTarget : ActionBase
    {
        [ApexSerialization(defaultValue = false)]
        public bool setMoveTarget;

        public override void Execute(IAIContext context)
        {
            var c = (UnitContext)context;

            var tempTarget = c.temporaryTarget;
            if (tempTarget == null)
            {
                // unit has no attack target
                return;
            }

            c.unit.MoveTo(tempTarget.position);

            if (this.setMoveTarget)
            {
                c.moveTarget = tempTarget.position;
            }
        }
    }
}