namespace Apex.Demo.RTS.AI
{
    using UnityEngine;

    /// <summary>
    /// Base class for all orders, defining common properties and abstract type properties.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.IOrder" />
    public abstract class OrderBase : IOrder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderBase"/> class.
        /// </summary>
        /// <param name="priority">The priority.</param>
        public OrderBase(int priority)
        {
            this.timestamp = Time.timeSinceLevelLoad;
            this.priority = priority;
        }

        /// <summary>
        /// Gets the order category.
        /// </summary>
        /// <value>
        /// The order category.
        /// </value>
        public abstract OrderCategoryType orderCategory { get; }

        /// <summary>
        /// Gets the specific type of order.
        /// </summary>
        /// <value>
        /// The type of the order.
        /// </value>
        public abstract OrderType orderType { get; }

        /// <summary>
        /// Gets the timestamp.
        /// </summary>
        /// <value>
        /// The timestamp.
        /// </value>
        public float timestamp
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the priority.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        public int priority
        {
            get;
            private set;
        }
    }
}