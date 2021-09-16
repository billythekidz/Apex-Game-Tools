namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;

    public sealed class ControllerHasEntityInMemoryWithinRange : ContextualScorerBase
    {
        [ApexSerialization(defaultValue = CompareOperator.LessThan), FriendlyName("Comparison", "Select what kind of comparison to do")]
        public CompareOperator comparison = CompareOperator.LessThan;

        [ApexSerialization, MemberDependency("useStructureGridSize", false)]
        public float customRange = 10f;

        [ApexSerialization(defaultValue = true)]
        public bool useStructureGridSize = true;

        [ApexSerialization(defaultValue = EntityType.Any)]
        public EntityType entityType = EntityType.Any;

        [ApexSerialization(defaultValue = false)]
        public bool ignoreAllies;

        [ApexSerialization(defaultValue = false)]
        public bool ignoreWorkers;

        [ApexSerialization(defaultValue = false), FriendlyName("Not", "Set to true to inverse the logic of this scorer, e.g. instead of scoring when true, it scores when false.")]
        public bool not;

        public override float Score(IAIContext context)
        {
            if (this.entityType == EntityType.None)
            {
                return 0f;
            }

            var c = (ControllerContext)context;
            var observations = this.ignoreAllies ? c.controller.enemyObservations : c.controller.observations;
            var count = observations.Count;
            if (count == 0)
            {
                return this.not ? this.score : 0f;
            }

            var range = this.customRange;
            if (this.useStructureGridSize)
            {
                range = (c.controller.structureGrid.size * Constants.SquareRootTwo);
            }

            var rangeSqr = range * range;

            var pos = c.controller.position;
            for (int i = 0; i < count; i++)
            {
                var obs = observations[i];
                if (this.entityType != EntityType.Any && obs.entity.entityType != this.entityType)
                {
                    // entity type does not match desired type
                    continue;
                }

                if (this.ignoreWorkers && this.entityType == EntityType.Unit)
                {
                    if (((IUnit)obs.entity).unitType == UnitType.Worker)
                    {
                        continue;
                    }
                }

                if (range <= 0f || Utils.IsOperatorTrue(this.comparison, (obs.entity.position - pos).sqrMagnitude, rangeSqr))
                {
                    return this.not ? 0f : this.score;
                }
            }

            return this.not ? this.score : 0f;
        }
    }
}