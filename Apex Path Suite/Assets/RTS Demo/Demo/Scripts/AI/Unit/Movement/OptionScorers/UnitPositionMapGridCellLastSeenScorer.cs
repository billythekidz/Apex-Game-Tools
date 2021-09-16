namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;
    using UnityEngine;

    public sealed class UnitPositionMapGridCellLastSeenScorer : OptionScorerBase<Vector3>
    {
        [ApexSerialization]
        public float maxScore = 100f;

        [ApexSerialization(defaultValue = true)]
        public bool reversed = true;

        public override float Score(IAIContext context, Vector3 option)
        {
            var c = (UnitContext)context;
            var cell = c.unit.controller.mapGrid.GetNearestCell(option);
            var timeFactor = cell.lastSeen / Time.timeSinceLevelLoad;
            return this.maxScore * (this.reversed ? (1f - timeFactor) : timeFactor);
        }
    }
}