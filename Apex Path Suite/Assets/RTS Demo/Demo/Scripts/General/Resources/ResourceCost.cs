namespace Apex.Demo.RTS.AI
{
    /// <summary>
    /// Represents a resource cost which may include a variable amount of <see cref="ResourceCostItem"/>s, as well as a time.
    /// The time dictates how long it takes to spend the resources e.g. constructing a structure or spawning a unit.
    /// </summary>
    public sealed class ResourceCost
    {
        /// <summary>
        /// The resources associated with this cost, can be a variable amount but should only be one per resource type.
        /// </summary>
        public ResourceCostItem[] resources;

        /// <summary>
        /// The time associated with this cost, for structures this means how long it takes to build it, for units how long it takes to spawn.
        /// </summary>
        public float time;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceCost"/> class.
        /// </summary>
        /// <param name="time">The time cost.</param>
        /// <param name="resources">The resource costs.</param>
        public ResourceCost(float time, params ResourceCostItem[] resources)
        {
            this.time = time;
            this.resources = resources;
        }
    }
}