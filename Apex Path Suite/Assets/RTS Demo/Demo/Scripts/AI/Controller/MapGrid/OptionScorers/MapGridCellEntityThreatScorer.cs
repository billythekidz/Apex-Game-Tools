namespace Apex.Demo.RTS.AI
{
    using Apex.AI;

    public sealed class MapGridCellEntityThreatScorer : OptionScorerBase<MapGridCell>
    {
        public override float Score(IAIContext context, MapGridCell cell)
        {
            var c = (ControllerContext)context;

            var entities = cell.entities;
            var threat = 0f;

            var count = entities.Count;
            for (int i = 0; i < count; i++)
            {
                var ent = entities[i];

                if (c.controller.IsEnemy(ent))
                {
                    threat += ent.threatFactor;
                }
                else
                {
                    threat -= ent.threatFactor;
                }
            }

            return threat;
        }
    }
}