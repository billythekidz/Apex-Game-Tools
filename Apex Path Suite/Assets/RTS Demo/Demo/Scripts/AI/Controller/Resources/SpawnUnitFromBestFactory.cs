namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Apex.Serialization;
    using Utilities;

    public sealed class SpawnUnitFromBestFactory : ActionWithOptions<Factory>
    {
        [ApexSerialization]
        public UnitType unitType = UnitType.Melee;

        public override void Execute(IAIContext context)
        {
            var c = (ControllerContext)context;
            var structures = c.controller.structures;
            var count = structures.Count;

            var list = ListBufferPool.GetBuffer<Factory>(count);

            for (int i = 0; i < count; i++)
            {
                var factory = structures[i].structureType == StructureType.Factory ? (Factory)structures[i] : null;
                if (factory == null)
                {
                    continue;
                }

                if (!factory.isReady)
                {
                    // factory not ready yet
                    continue;
                }

                list.Add(factory);
            }

            var best = this.GetBest(c, list);
            if (best != null)
            {
                best.SpawnUnit(this.unitType);
            }

            ListBufferPool.ReturnBuffer(list);
        }
    }
}