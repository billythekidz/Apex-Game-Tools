namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using UnityEngine;

    public sealed class UpdateMapGridThreatMax : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            var c = (ControllerContext)context;
            var mapGrid = c.controller.mapGrid;
            var cells = mapGrid.cells;

            var max = float.MinValue;
            for (int i = 0; i < cells.Length; i++)
            {
                var threat = Mathf.Abs(cells[i].threat);
                if (threat > max)
                {
                    max = threat;
                }
            }

            mapGrid.absoluteMax = max;
        }
    }
}