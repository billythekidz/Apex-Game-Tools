namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using UnityEngine;

    public sealed class StructureScanner : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            var c = (StructureContext)context;
            var structure = c.structure as IHasScanner;
            if (structure == null)
            {
                // structure has no scanner
                return;
            }

            // use OverlapSphere for scanning
            var layers = LayersHelper.instance;
            var hits = Utils.colliderBuffer;
            Physics.OverlapSphereNonAlloc(structure.position, structure.scanRadius, hits, layers.unitLayer | layers.resourceLayer | layers.structureLayer, QueryTriggerInteraction.Collide);

            var controller = c.structure.controller;
            for (int i = 0; i < hits.Length; i++)
            {
                var hit = hits[i];
                if (hit == null)
                {
                    continue;
                }

                if (hit.gameObject == c.structure.gameObject)
                {
                    // ignore hits with self
                    continue;
                }

                var entity = hit.GetEntity();
                if (entity == null)
                {
                    // hit is somehow not an entity
                    continue;
                }

                // send the observation to the controller
                controller.AddObservation(entity);
            }
        }
    }
}