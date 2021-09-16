namespace Apex.Demo.RTS.AI
{
    /// <summary>
    /// Represents a single resource cost. Specifies the cost amount and the type of resource.
    /// </summary>
    public struct ResourceCostItem
    {
        /// <summary>
        /// The type of resource that this cost defines.
        /// </summary>
        public ResourceType type;

        /// <summary>
        /// The quantity of resources that this cost defines.
        /// </summary>
        public int quantity;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceCostItem"/> struct.
        /// </summary>
        /// <param name="type">The cost type.</param>
        /// <param name="quantity">The cost quantity.</param>
        public ResourceCostItem(ResourceType type, int quantity)
        {
            this.type = type;
            this.quantity = quantity;
        }
    }
}