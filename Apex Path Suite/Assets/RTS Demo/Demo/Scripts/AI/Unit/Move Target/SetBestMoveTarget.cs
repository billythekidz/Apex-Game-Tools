namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using UnityEngine;

    public class SetBestMoveTarget : ActionWithOptions<Vector3>
    {
        public override void Execute(IAIContext context)
        {
            var c = (UnitContext)context;
            var positions = c.sampledPositions;
            if (positions.Count == 0)
            {
                //  no sampled positions available
                return;
            }

            var best = this.GetBest(c, positions);
            c.moveTarget = best;
        }
    }
}