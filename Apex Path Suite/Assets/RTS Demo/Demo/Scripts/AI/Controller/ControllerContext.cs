namespace Apex.Demo.RTS.AI
{
    using Apex.AI;

    public sealed class ControllerContext : IAIContext
    {
        private Center _center;

        public ControllerContext(AIController controller)
        {
            this.controller = controller;
        }

        public AIController controller
        {
            get;
            private set;
        }

        public Center center
        {
            get { return _center ?? (_center = controller.center); }
        }

        public UnitGroup targetGroup
        {
            get;
            set;
        }
    }
}