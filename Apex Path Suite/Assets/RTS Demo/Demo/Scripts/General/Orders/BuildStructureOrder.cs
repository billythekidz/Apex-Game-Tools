namespace Apex.Demo.RTS.AI
{
    using UnityEngine;

    /// <summary>
    /// Represents a build structure order. Issued exclusively to worker units. Specifices a location and type of structure to build. 
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.OrderBase" />
    public sealed class BuildStructureOrder : OrderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuildStructureOrder"/> class.
        /// </summary>
        /// <param name="priority">The priority.</param>
        /// <param name="type">The type.</param>
        /// <param name="location">The location - if the location is null, the order assumes that the unit will build whereever it is. </param>
        public BuildStructureOrder(int priority, StructureType type, Vector3 location)
            : base(priority)
        {
            this.structureType = type;
            this.location = location;
        }

        /// <summary>
        /// Gets the order category.
        /// </summary>
        /// <value>
        /// The order category.
        /// </value>
        public override OrderCategoryType orderCategory
        {
            get { return OrderCategoryType.Construction; }
        }

        /// <summary>
        /// Gets the specific type of order.
        /// </summary>
        /// <value>
        /// The type of the order.
        /// </value>
        public override OrderType orderType
        {
            get { return OrderType.BuildStructure; }
        }

        /// <summary>
        /// The type of structure to build.
        /// </summary>
        public StructureType structureType
        {
            get;
            private set;
        }

        /// <summary>
        /// The location to build the structure at.
        /// </summary>
        public Vector3 location
        {
            get;
            private set;
        }
    }
}