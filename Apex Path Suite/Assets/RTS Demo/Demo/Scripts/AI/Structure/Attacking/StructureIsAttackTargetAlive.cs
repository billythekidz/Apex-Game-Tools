﻿namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Apex.Serialization;

    public sealed class StructureIsAttackTargetAlive : ContextualScorerBase
    {
        [ApexSerialization(defaultValue = false), FriendlyName("Not", "Set to true to inverse the logic of this scorer, e.g. instead of scoring when true, it scores when false.")]
        public bool not;

        public override float Score(IAIContext context)
        {
            var c = (StructureContext)context;
            var attackTarget = c.attackTarget;
            if (attackTarget == null)
            {
                return 0f;
            }

            if (!attackTarget.isDead)
            {
                // attack target is alive
                return this.not ? 0f : this.score;
            }

            return this.not ? this.score : 0f;
        }
    }
}