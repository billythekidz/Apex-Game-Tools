namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;
    using UnityEngine;

    public sealed class MapGridCellNeighbourCellThreatScorer : OptionScorerBase<MapGridCell>
    {
        [ApexSerialization]
        public AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        [ApexSerialization]
        public float influenceRadius = 20f;

        public override float Score(IAIContext context, MapGridCell cell)
        {
            var c = (ControllerContext)context;
            var mapGrid = c.controller.mapGrid;
            if (mapGrid.absoluteMax == 0f)
            {
                // cannot calculate the neighbour cell threat score if the absolute max is 0 (all cells have threat 0)
                return 0f;
            }

            var radiusSqr = influenceRadius * influenceRadius;
            var summedThreat = 0f;

            var cells = mapGrid.cells;
            var count = cells.Length;
            for (int i = 0; i < count; i++)
            {
                var distanceSqr = (cells[i].center - cell.center).sqrMagnitude;
                if (distanceSqr > radiusSqr)
                {
                    continue;
                }

                var threat = cells[i].threat;
                if (threat == 0f)
                {
                    continue;
                }

                // the distance factor is calculated so that nearby grid cells affect more than those further away, even if they are within radius
                var distanceFactor = 1f - (distanceSqr / radiusSqr);

                // calculate the normalized threat (only positive values), then evaluate the positive values on the curve, before applying the sign of the threat
                var normalizedThreat = Mathf.Abs(threat * distanceFactor) / mapGrid.absoluteMax;
                summedThreat += Mathf.Sign(threat) * this.curve.Evaluate(normalizedThreat);
            }

            return summedThreat;
        }
    }
}