namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Apex.Serialization;

    public sealed class MoveToGuardTarget : ActionBase
    {
        [ApexSerialization(defaultValue = false)]
        public bool setMoveTarget;

        public override void Execute(IAIContext context)
        {
            var c = (UnitContext)context;
            var guardTarget = c.guardTarget;
            if (!guardTarget.HasValue)
            {
                // unit has no attack target
                return;
            }

            c.unit.MoveTo(guardTarget.Value);

            if (this.setMoveTarget)
            {
                c.moveTarget = guardTarget.Value;
            }
        }
    }
}