namespace Apex.Demo.RTS.AI
{
    using System.Collections.Generic;
    using Apex.AI;
    using Serialization;

    public sealed class HasEntityInMemory : UnitRangeContextualScorerBase
    {
        [ApexSerialization(defaultValue = EntityType.Any)]
        public EntityType entityType = EntityType.Any;

        [ApexSerialization(defaultValue = false)]
        public bool ignoreAllies;

        [ApexSerialization(defaultValue = false)]
        public bool useControllerMemory;

        [ApexSerialization(defaultValue = false)]
        public bool ignoreWorkers;

        [ApexSerialization(defaultValue = false), FriendlyName("Not", "Set to true to inverse the logic of this scorer, e.g. instead of scoring when true, it scores when false.")]
        public bool not;

        public override float Score(IAIContext context)
        {
            var c = (UnitContext)context;

            IList<Observation> observations;
            if (this.useControllerMemory)
            {
                observations = this.ignoreAllies ? c.unit.controller.enemyObservations : c.unit.controller.observations;
            }
            else
            {
                observations = this.ignoreAllies ? c.unit.enemyObservations : c.unit.observations;
            }

            var count = observations.Count;
            if (count == 0)
            {
                return 0f;
            }

            for (int i = 0; i < count; i++)
            {
                var entity = observations[i].entity;
                if (this.entityType != EntityType.Any && entity.entityType != this.entityType)
                {
                    // entity type does not match desired type
                    continue;
                }

                if (this.ignoreWorkers && this.entityType == EntityType.Unit)
                {
                    if (((IUnit)entity).unitType == UnitType.Worker)
                    {
                        continue;
                    }
                }

                if (IsComparisonTrue(c, (c.unit.position - entity.position).sqrMagnitude))
                {
                    return this.not ? 0f : this.score;
                }
            }

            return this.not ? this.score : 0f;
        }
    }
}