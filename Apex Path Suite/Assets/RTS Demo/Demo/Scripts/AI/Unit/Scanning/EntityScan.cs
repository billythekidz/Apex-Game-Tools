namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;
    using UnityEngine;

    public sealed class EntityScan : ActionBase
    {
        [ApexSerialization]
        public float observationVisibilityThreshold = 1.5f;

        public override void Execute(IAIContext context)
        {
            var c = (UnitContext)context;

            // use OverlapSphere for scanning
            var layers = LayersHelper.instance;
            var hits = Utils.colliderBuffer;
            Physics.OverlapSphereNonAlloc(c.unit.position, c.unit.scanRadius, hits, layers.unitLayer | layers.resourceLayer | layers.structureLayer, QueryTriggerInteraction.Collide);
            var timestamp = Time.timeSinceLevelLoad;

            var enemyObservations = c.unit.enemyObservations;
            var enemyCount = enemyObservations.Count;
            var observations = c.unit.observations;
            var count = observations.Count;
            for (int i = 0; i < hits.Length; i++)
            {
                var hit = hits[i];
                if (hit == null)
                {
                    continue;
                }

                if (hit.gameObject == c.unit.gameObject)
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

                Observation obs = null;
                var existing = false;
                for (int j = 0; j < count; j++)
                {
                    // must compare the pool IDs since entities are reused through instance pooling
                    if (observations[j].entity.id == entity.id)
                    {
                        // existing observation exists
                        obs = observations[j];
                        obs.position = entity.position;
                        obs.timestamp = timestamp;

                        var hasHealthObs = obs.GetEntity<IHasHealth>();
                        if (hasHealthObs != null && c.unit.IsEnemy(hasHealthObs))
                        {
                            // update enemy observation
                            for (int h = 0; h < enemyCount; h++)
                            {
                                var enemyObs = enemyObservations[h];
                                if (enemyObs.entity.id == entity.id)
                                {
                                    enemyObs.position = entity.position;
                                    enemyObs.timestamp = timestamp;
                                }
                            }
                        }

                        existing = true;
                        break;
                    }
                }

                if (!existing)
                {
                    // no existing observation exists
                    obs = new Observation(entity);
                    c.unit.AddObservation(obs);
                }

                c.unit.controller.AddObservation(obs);
            }

            // need to get count again, since the count could have changed
            count = observations.Count;
            for (int h = count - 1; h >= 0; h--)
            {
                var obs = observations[h];
                if (obs.entity == null || !obs.entity.active)
                {
                    // game object no longer active, so remove it from observations
                    observations.RemoveAt(h);
                }
            }

            // clean up in enemy observations
            enemyCount = enemyObservations.Count;
            for (int k = enemyCount - 1; k >= 0; k--)
            {
                var obs = enemyObservations[k];
                if (obs.entity == null || !obs.entity.active)
                {
                    // game object no longer active, so remove it from observations
                    enemyObservations.RemoveAt(k);
                }
            }
        }
    }
}