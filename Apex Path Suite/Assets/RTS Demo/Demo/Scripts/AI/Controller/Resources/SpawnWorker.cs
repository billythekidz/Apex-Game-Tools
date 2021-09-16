namespace Apex.Demo.RTS.AI
{
    using Apex.AI;

    public sealed class SpawnWorker : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            ((ControllerContext)context).center.SpawnWorker();
        }
    }
}