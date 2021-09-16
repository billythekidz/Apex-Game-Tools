namespace Apex.Demo.RTS.AI
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface representing entities that can receive orders.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.IEntity" />
    public interface IHasOrders : IEntity
    {
        /// <summary>
        /// Gets the list of non-completed orders.
        /// </summary>
        /// <value>
        /// The orders.
        /// </value>
        IList<IOrder> orders { get; }

        /// <summary>
        /// Gets the list of completed orders.
        /// </summary>
        /// <value>
        /// The completed orders.
        /// </value>
        IList<IOrder> completedOrders { get; }

        /// <summary>
        /// Gets or sets the current order.
        /// </summary>
        /// <value>
        /// The current order.
        /// </value>
        IOrder currentOrder { get; set; }

        /// <summary>
        /// Gets or sets the currently executing order.
        /// </summary>
        /// <value>
        /// The currently executing order.
        /// </value>
        IOrder currentlyExecuting { get; set; }
    }
}