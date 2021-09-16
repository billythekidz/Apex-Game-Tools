namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;

    public sealed class WorkerSetFleeing : ActionBase
    {
        [ApexSerialization(defaultValue = false)]
        public bool fleeing;

        public override void Execute(IAIContext context)
        {
            var c = (UnitContext)context;
            var worker = c.unit as WorkerUnit;
            if (worker == null)
            {
                return;
            }

            worker.isFleeing = fleeing;
        }
    }
}