namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Apex.Serialization;

    public sealed class MoveToAttackTarget : ActionBase
    {
        [ApexSerialization(defaultValue = false)]
        public bool setMoveTarget;

        public override void Execute(IAIContext context)
        {
            var c = (UnitContext)context;

            var attackTarget = c.attackTarget;
            if (attackTarget == null)
            {
                // unit has no attack target
                return;
            }

            c.unit.MoveTo(attackTarget.position);

            if (this.setMoveTarget)
            {
                c.moveTarget = attackTarget.position;
            }
        }
    }
}