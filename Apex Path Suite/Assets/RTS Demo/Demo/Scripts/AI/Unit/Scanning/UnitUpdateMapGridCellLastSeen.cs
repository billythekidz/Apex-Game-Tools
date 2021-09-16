namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using UnityEngine;

    public sealed class UnitUpdateMapGridCellLastSeen : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            var c = (UnitContext)context;
            var cell = c.unit.controller.mapGrid.GetNearestCell(c.unit.position);
            if (cell == null)
            {
                return;
            }

            cell.lastSeen = Time.timeSinceLevelLoad;
        }
    }
}