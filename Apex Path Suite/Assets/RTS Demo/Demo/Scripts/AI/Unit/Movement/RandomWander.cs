namespace Apex.Demo.RTS.AI
{
    using Apex.AI;

    public sealed class RandomWander : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            ((UnitContext)context).unit.RandomWander();
        }
    }
}