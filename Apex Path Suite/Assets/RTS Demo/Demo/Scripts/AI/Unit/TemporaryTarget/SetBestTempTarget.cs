namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;
    using UnityEngine;
    using Utilities;

    public sealed class SetBestTempTarget : ActionWithOptions<IHasHealth>
    {
        [ApexSerialization(defaultValue = 10f)]
        public float lastAttackedThreshold = 10f;

        [ApexSerialization(defaultValue = false)]
        public bool useControllerMemory;

        public override void Execute(IAIContext context)
        {
            var c = (UnitContext)context;
            var observations = this.useControllerMemory ? c.unit.controller.enemyObservations : c.unit.enemyObservations;
            var count = observations.Count;

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

            if (c.unit.lastAttacker != null && (Time.timeSinceLevelLoad - c.unit.lastAttacked) < this.lastAttackedThreshold)
            {
                // add the last attacker to the candidates if it is available and recent
                list.Add(c.unit.lastAttacker);
            }

            var best = this.GetBest(c, list);
            if (best != null)
            {
                c.temporaryTarget = best;
            }

            ListBufferPool.ReturnBuffer(list);
        }
    }
}