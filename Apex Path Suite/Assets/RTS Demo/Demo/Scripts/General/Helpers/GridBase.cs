namespace Apex.Demo.RTS.AI
{
    using UnityEngine;

    /// <summary>
    /// A base class for easily defining grids of the different types.
    /// </summary>
    /// <typeparam name="T">The type of cells encompassed by the grid.</typeparam>
    public abstract class GridBase<T> where T : GridCellBase
    {
        protected T[] _cells;

        /// <summary>
        /// Creates an instance of <see cref="GridBase{T}"/>.
        /// </summary>
        /// <param name="center">The center position of the grid.</param>
        /// <param name="size">The size of the grid around the supplied center.</param>
        /// <param name="spacing">The spacing between individual cells.</param>
        /// <param name="cellSize">The size of cells.</param>
        public GridBase(Vector3 center, float size, float spacing, float cellSize)
        {
            this.size = size;
            this.spacing = spacing;
            this.cellSize = cellSize;
            Init(center);
        }

        /// <summary>
        /// Array of all cells in the grid.
        /// </summary>
        public T[] cells
        {
            get { return _cells; }
        }

        /// <summary>
        /// The size of the grid.
        /// </summary>
        public float size
        {
            get;
            private set;
        }

        /// <summary>
        /// The cell size for each cell in the grid.
        /// </summary>
        public float cellSize
        {
            get;
            private set;
        }

        /// <summary>
        /// The spacing between individual grid cells.
        /// </summary>
        public float spacing
        {
            get;
            private set;
        }

        /// <summary>
        /// Initialize the grid at the given center.
        /// </summary>
        /// <param name="center"></param>
        protected abstract void Init(Vector3 center);

        /// <summary>
        /// Get the nearest cell compared to the supplied position.
        /// </summary>
        /// <param name="position"></param>
        /// <returns>The nearest cell to the suplied position.</returns>
        public T GetNearestCell(Vector3 position)
        {
            T nearest = null;
            var shortest = float.MaxValue;
            for (int i = 0; i < _cells.Length; i++)
            {
                var distanceSqr = (_cells[i].center - position).sqrMagnitude;
                if (distanceSqr < shortest)
                {
                    shortest = distanceSqr;
                    nearest = _cells[i];
                }
            }

            return nearest;
        }

        /// <summary>
        /// Get the cell covered by the supplied <seealso cref="Bounds"/> object.
        /// </summary>
        /// <param name="bounds">The bounds object to check for cell bounds intersection.</param>
        /// <returns>The cell whose bounds intersects with the supplied Bounds object.</returns>
        public T GetCellIn(Bounds bounds)
        {
            for (int i = 0; i < _cells.Length; i++)
            {
                if (_cells[i].bounds.Intersects(bounds))
                {
                    return _cells[i];
                }
            }

            return null;
        }
    }
}