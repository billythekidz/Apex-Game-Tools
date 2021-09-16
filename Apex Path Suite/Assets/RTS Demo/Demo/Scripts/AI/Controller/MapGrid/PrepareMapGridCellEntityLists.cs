namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using UnityEngine;

    public sealed class PrepareMapGridCellEntityLists : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            var c = (ControllerContext)context;
            var observations = c.controller.observations;
            var count = observations.Count;
            if (count == 0)
            {
                return;
            }

            var time = Time.timeSinceLevelLoad;
            c.controller.mapGrid.ClearEntityLists();
            var cells = c.controller.mapGrid.cells;
            for (int j = 0; j < cells.Length; j++)
            {
                var cell = cells[j];

                for (int i = 0; i < count; i++)
                {
                    var obs = observations[i];
                    var hasHealth = obs.GetEntity<IHasHealth>();
                    if (hasHealth == null)
                    {
                        // ignore non-killable entities
                        continue;
                    }

                    if (cell.bounds.Contains(obs.position))
                    {
                        cell.entities.Add(hasHealth);
                        cell.lastSeen = time;
                    }
                }
            }
        }
    }
}