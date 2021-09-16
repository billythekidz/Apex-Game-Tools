namespace Apex.Demo.RTS.AI
{
    /// <summary>
    /// Interface representing all orders.
    /// </summary>
    public interface IOrder
    {
        /// <summary>
        /// Gets the order category.
        /// </summary>
        /// <value>
        /// The order category.
        /// </value>
        OrderCategoryType orderCategory { get; }

        /// <summary>
        /// Gets the specific type of order.
        /// </summary>
        /// <value>
        /// The type of the order.
        /// </value>
        OrderType orderType { get; }

        /// <summary>
        /// Gets the timestamp.
        /// </summary>
        /// <value>
        /// The timestamp.
        /// </value>
        float timestamp { get; }

        /// <summary>
        /// Gets the priority.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        int priority { get; }
    }
}