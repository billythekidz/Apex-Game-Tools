namespace Apex.Demo.RTS.AI
{
    using Apex.AI;

    public sealed class StructureAttack : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            var c = (StructureContext)context;
            var structure = c.structure as CannonTower;
            if (structure == null)
            {
                // only supported for cannon towers currently
                return;
            }

            if (c.attackTarget == null)
            {
                return;
            }

            structure.Attack(c.attackTarget);
        }
    }
}