namespace Apex.Demo.RTS.AI
{
    /// <summary>
    /// Represents scout orders.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.OrderBase" />
    public sealed class ScoutOrder : OrderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScoutOrder"/> class.
        /// </summary>
        /// <param name="priority">The priority.</param>
        public ScoutOrder(int priority)
            : base(priority)
        {
        }

        /// <summary>
        /// Gets the order category.
        /// </summary>
        /// <value>
        /// The order category.
        /// </value>
        public override OrderCategoryType orderCategory
        {
            get { return OrderCategoryType.Scouting; }
        }

        /// <summary>
        /// Gets the specific type of order.
        /// </summary>
        /// <value>
        /// The type of the order.
        /// </value>
        public override OrderType orderType
        {
            get { return OrderType.Scout; }
        }
    }
}