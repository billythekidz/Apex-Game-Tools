namespace Apex.Demo.RTS.AI
{
    using System.Collections.Generic;
    using Apex.AI;
    using Serialization;
    using Utilities;

    public sealed class SetBestResourceTarget : ActionWithOptions<IResource>
    {
        [ApexSerialization(defaultValue = false)]
        public bool useControllerMemory;

        public override void Execute(IAIContext context)
        {
            var c = (UnitContext)context;
            var unit = c.unit as WorkerUnit;
            if (unit == null)
            {
                // only worker units can have resource targets
                return;
            }

            IList<Observation> observations;
            if (this.useControllerMemory)
            {
                observations = c.unit.controller.observations;
            }
            else
            {
                observations = c.unit.observations;
            }

            var count = observations.Count;
            if (count == 0)
            {
                return;
            }

            var list = ListBufferPool.GetBuffer<IResource>(count);
            for (int i = 0; i < count; i++)
            {
                var obs = observations[i];
                if (obs.entity.entityType != EntityType.Resource)
                {
                    continue;
                }

                list.Add((IResource)obs.entity);
            }

            var best = this.GetBest(c, list);
            if (best != null)
            {
                unit.resourceTarget = best;
            }

            ListBufferPool.ReturnBuffer(list);
        }
    }
}