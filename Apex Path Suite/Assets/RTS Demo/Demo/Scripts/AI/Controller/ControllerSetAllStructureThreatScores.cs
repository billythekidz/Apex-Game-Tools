namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Utilities;

    public sealed class ControllerSetAllStructureThreatScores : ActionWithOptions<IStructure>
    {
        public override void Execute(IAIContext context)
        {
            var c = (ControllerContext)context;

            var list = ListBufferPool.GetBuffer<ScoredOption<IStructure>>(c.controller.structures.Count);
            this.GetAllScores(c, c.controller.structures, list);

            var count = list.Count;
            for (int i = 0; i < count; i++)
            {
                list[i].option.threatScore = list[i].score;
            }

            ListBufferPool.ReturnBuffer(list);
        }
    }
}