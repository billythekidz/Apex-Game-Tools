namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;

    public sealed class SetStrategy : ActionBase
    {
        [ApexSerialization]
        public StrategyType strategyType;

        public override void Execute(IAIContext context)
        {
            ((ControllerContext)context).controller.strategy = this.strategyType;
        }
    }
}