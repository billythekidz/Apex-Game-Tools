namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;

    public sealed class StructureThreatScorer : OptionScorerBase<IStructure>
    {
        [ApexSerialization, MemberDependency("recalculate", true)]
        public float influenceRange = 25f;

        [ApexSerialization(defaultValue = 1f), MemberDependency("recalculate", false)]
        public float factor = 1f;

        [ApexSerialization(defaultValue = false)]
        public bool recalculate;

        public override float Score(IAIContext context, IStructure structure)
        {
            if (!this.recalculate)
            {
                // if not recalculating, just return the last threat score
                return structure.threatScore * this.factor;
            }

            var c = (ControllerContext)context;
            var controller = c.controller;
            var observations = controller.observations;
            var count = observations.Count;
            if (count == 0)
            {
                return 0f;
            }

            var radiusSqr = this.influenceRange * this.influenceRange;
            var score = structure.threatPriority;
            for (int i = 0; i < count; i++)
            {
                var obs = observations[i];
                var distance = (structure.position - obs.position).sqrMagnitude;
                if (distance > radiusSqr)
                {
                    // outside of influence range
                    continue;
                }

                var ent = obs.GetEntity<IHasHealth>();
                if (ent == null)
                {
                    // not an entity
                    continue;
                }

                if (controller.IsEnemy(ent))
                {
                    // add to threat score for each enemy
                    score += ent.threatFactor;
                }
                else
                {
                    // subtract from threat score for each ally
                    score -= ent.threatFactor;
                }
            }

            return score;
        }
    }
}