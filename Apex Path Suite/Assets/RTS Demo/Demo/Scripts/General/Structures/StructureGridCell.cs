namespace Apex.Demo.RTS.AI
{
    using UnityEngine;

    /// <summary>
    /// Represents a single cell in a structure grid. Has the basic properties of a normal cell, and the occupied flag.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.GridCellBase" />
    public sealed class StructureGridCell : GridCellBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StructureGridCell"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="size">The size of the cell.</param>
        public StructureGridCell(Vector3 position, float size)
            : base(position, size)
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="StructureGridCell"/> is occupied.
        /// </summary>
        /// <value>
        ///   <c>true</c> if occupied; otherwise, <c>false</c>.
        /// </value>
        public bool occupied
        {
            get;
            set;
        }
    }
}