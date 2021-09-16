namespace Apex.Demo.RTS.AI
{
    using Apex.AI;

    public sealed class StructureContext : IAIContext
    {
        public StructureContext(IStructure structure)
        {
            this.structure = structure;
        }

        public IStructure structure
        {
            get;
            private set;
        }

        public IHasHealth attackTarget
        {
            get;
            set;
        }
    }
}