namespace Apex.Demo.RTS.AI
{
    using UnityEngine;

    /// <summary>
    /// Base class for grid cells. Represents a single cell in a grid.
    /// </summary>
    public abstract class GridCellBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GridCellBase"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="size">The size.</param>
        public GridCellBase(Vector3 position, float size)
        {
            this.center = position;
            this.size = size;

            // The bounds object is always relatively 'flat' for grid cells, in that the Y value is low regardless of the x-z size.
            var s = size * Vector3.one;
            s.y = 1f;
            this.bounds = new Bounds(position, s);
        }

        /// <summary>
        /// Gets the bounds of the cell.
        /// </summary>
        /// <value>
        /// The bounds.
        /// </value>
        public Bounds bounds
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the size of the cell.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public float size
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the center position of the cell.
        /// </summary>
        /// <value>
        /// The center.
        /// </value>
        public Vector3 center
        {
            get;
            private set;
        }
    }
}