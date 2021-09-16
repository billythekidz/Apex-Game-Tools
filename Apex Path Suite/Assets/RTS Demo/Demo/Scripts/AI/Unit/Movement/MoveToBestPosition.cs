namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;
    using UnityEngine;

    public sealed class MoveToBestPosition : ActionWithOptions<Vector3>
    {
        [ApexSerialization(defaultValue = false)]
        public bool setMoveTarget;

        public override void Execute(IAIContext context)
        {
            var c = (UnitContext)context;
            var positions = c.sampledPositions;
            if (positions.Count == 0)
            {
                // no sampled positions available
                return;
            }

            var best = this.GetBest(c, positions);
            c.unit.MoveTo(best);

            if (this.setMoveTarget)
            {
                c.moveTarget = best;
            }
        }
    }
}