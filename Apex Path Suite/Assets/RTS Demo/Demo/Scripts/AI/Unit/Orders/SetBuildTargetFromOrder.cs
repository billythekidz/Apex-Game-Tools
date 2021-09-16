namespace Apex.Demo.RTS.AI
{
    using Apex.AI;

    public sealed class SetBuildTargetFromOrder : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            var c = (UnitContext)context;
            var order = c.unit.currentOrder as BuildStructureOrder;
            if (order == null)
            {
                return;
            }

            c.buildTarget = new BuildTarget(order.structureType, order.location);
        }
    }
}