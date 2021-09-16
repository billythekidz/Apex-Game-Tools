namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Apex.Serialization;

    public sealed class BuildStructureAction : ActionBase
    {
        [ApexSerialization, MemberDependency("getTypeFromBuildTarget", false)]
        public StructureType type;

        [ApexSerialization, MemberDependency("type", 0, MaskMatch.Equals)]
        public bool getTypeFromBuildTarget;

        public override void Execute(IAIContext context)
        {
            var c = (UnitContext)context;
            var unit = c.unit as WorkerUnit;
            if (unit == null)
            {
                // only worker units can build
                return;
            }

            if (this.getTypeFromBuildTarget)
            {
                unit.BuildStructure(c.buildTarget.type);
                return;
            }

            unit.BuildStructure(this.type);
        }
    }
}