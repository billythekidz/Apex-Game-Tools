namespace Apex.Demo.RTS.AI
{
    using UnityEngine;

    /// <summary>
    /// The map grid is a grid of <seealso cref="MapGridCell"/>s covering the entire map.
    /// Each cell has a timestamp for when that cell was last seen by one of the AI's units.
    /// Additionally, the map grid serves as an influence map for evaluating threat levels.
    /// </summary>
    public sealed class MapGrid : GridBase<MapGridCell>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapGrid"/> class.
        /// </summary>
        /// <param name="center">The center position of the grid.</param>
        /// <param name="size">The size of the grid around the supplied center.</param>
        /// <param name="spacing">The spacing between individual cells.</param>
        /// <param name="cellSize">The size of cells.</param>
        public MapGrid(Vector3 center, float size, float spacing, float cellSize)
            : base(center, size, spacing, cellSize)
        {
        }

        /// <summary>
        /// Gets or sets the absolute maximum threat value - the highest threat score (negative or positive) on the map grid.
        /// </summary>
        /// <value>
        /// The absolute maximum.
        /// </value>
        public float absoluteMax
        {
            get;
            set;
        }

        /// <summary>
        /// Initialize the grid at the given center.
        /// </summary>
        /// <param name="center"></param>
        protected override void Init(Vector3 center)
        {
            var space = this.spacing + this.cellSize;
            var oneSideSize = Mathf.CeilToInt(this.size / space);
            _cells = new MapGridCell[oneSideSize * oneSideSize];
            var halfSize = this.size * 0.5f;
            var index = 0;

            for (float x = center.x - halfSize; x <= center.x + halfSize; x += space)
            {
                for (float z = center.z - halfSize; z <= center.z + halfSize; z += space)
                {
                    _cells[index++] = new MapGridCell(new Vector3(x, center.y, z), this.cellSize);
                }
            }
        }

        /// <summary>
        /// Each cell has a list of entities currently residing in that cell (to calculate threat). This method clears those lists.
        /// </summary>
        public void ClearEntityLists()
        {
            for (int i = 0; i < _cells.Length; i++)
            {
                _cells[i].entities.Clear();
            }
        }
    }
}