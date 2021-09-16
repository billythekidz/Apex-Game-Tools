namespace Apex.Demo.RTS.AI
{
    public sealed class RetreatToBaseOrder : OrderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RetreatToBaseOrder"/> class.
        /// </summary>
        /// <param name="priority">The priority.</param>
        public RetreatToBaseOrder(int priority)
            : base(priority)
        {
        }

        /// <summary>
        /// The category of the order.
        /// </summary>
        public override OrderCategoryType orderCategory
        {
            get { return OrderCategoryType.Defensive; }
        }

        /// <summary>
        /// The specific type of order.
        /// </summary>
        public override OrderType orderType
        {
            get { return OrderType.RetreatToBase; }
        }
    }
}