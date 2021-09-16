namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;
    using Utilities;

    public sealed class SetBestAttackTarget : ActionWithOptions<IHasHealth>
    {
        [ApexSerialization(defaultValue = false)]
        public bool useControllerMemory;

        public override void Execute(IAIContext context)
        {
            var c = (UnitContext)context;
            var observations = this.useControllerMemory ? c.unit.controller.enemyObservations : c.unit.enemyObservations;
            var count = observations.Count;
            if (count == 0)
            {
                // no observations to set as attack target
                return;
            }

            var list = ListBufferPool.GetBuffer<IHasHealth>(count);
            for (int i = 0; i < count; i++)
            {
                var obs = observations[i];
                var hasHealth = obs.GetEntity<IHasHealth>();
                if (hasHealth == null)
                {
                    // observation is not an entity that can die, thus ignore it
                    continue;
                }

                list.Add(hasHealth);
            }

            var best = this.GetBest(c, list);
            if (best != null)
            {
                c.attackTarget = best;
            }

            ListBufferPool.ReturnBuffer(list);
        }
    }
}