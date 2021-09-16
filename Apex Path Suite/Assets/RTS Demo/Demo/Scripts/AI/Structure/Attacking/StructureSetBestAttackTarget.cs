namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using UnityEngine;
    using Utilities;

    public sealed class StructureSetBestAttackTarget : ActionWithOptions<IHasHealth>
    {
        public override void Execute(IAIContext context)
        {
            var c = (StructureContext)context;
            var scan = c.structure as IHasScanner;
            if (scan == null)
            {
                // structure cannot scan
                return;
            }

            var layers = LayersHelper.instance;
            var hits = Utils.colliderBuffer;
            Physics.OverlapSphereNonAlloc(c.structure.position, scan.scanRadius, hits, layers.unitLayer | layers.structureLayer, QueryTriggerInteraction.Collide);

            var list = ListBufferPool.GetBuffer<IHasHealth>(hits.Length);
            for (int i = 0; i < hits.Length; i++)
            {
                var hit = hits[i];
                if (hit == null)
                {
                    continue;
                }

                var ent = hit.GetEntity<IHasHealth>();
                if (ent == null)
                {
                    // cannot be damaged
                    continue;
                }

                if (ent.id == c.structure.id)
                {
                    // cannot damage self
                    continue;
                }

                if (c.structure.IsAlly(ent))
                {
                    // ignore allies
                    continue;
                }

                list.Add(ent);
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