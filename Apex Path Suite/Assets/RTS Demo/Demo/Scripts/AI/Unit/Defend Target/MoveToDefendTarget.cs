namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Apex.Serialization;

    public sealed class MoveToDefendTarget : ActionBase
    {
        [ApexSerialization(defaultValue = false)]
        public bool setMoveTarget;

        public override void Execute(IAIContext context)
        {
            var c = (UnitContext)context;
            var defendTarget = c.defendTarget;
            if (defendTarget == null)
            {
                // unit has no attack target
                return;
            }

            c.unit.MoveTo(defendTarget.position);

            if (this.setMoveTarget)
            {
                c.moveTarget = defendTarget.position;
            }
        }
    }
}