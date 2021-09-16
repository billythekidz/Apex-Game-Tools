namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Utilities;

    public sealed class SetAllMapThreatScores : ActionWithOptions<MapGridCell>
    {
        public override void Execute(IAIContext context)
        {
            var c = (ControllerContext)context;
            var cells = c.controller.mapGrid.cells;

            var list = ListBufferPool.GetBuffer<ScoredOption<MapGridCell>>(cells.Length);
            this.GetAllScores(c, cells, list);

            var count = list.Count;
            for (int i = 0; i < count; i++)
            {
                list[i].option.threat = list[i].score;
            }

            ListBufferPool.ReturnBuffer(list);
        }
    }
}