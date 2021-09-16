namespace Apex.Demo.RTS.AI
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// A single cell in a map grid.
    /// The cell has a timestamp for when it was last seen, as well as a calculated threat level, based on the entities currently residing inside the cell.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.GridCellBase" />
    public sealed class MapGridCell : GridCellBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapGridCell"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="size">The size.</param>
        public MapGridCell(Vector3 position, float size)
            : base(position, size)
        {
            this.entities = new List<IHasHealth>(20); // TODO: Better preallocation?
        }

        /// <summary>
        /// Gets or sets the calculated threat level for this cell. Negative threat levels means 'no threat', while positive threat levels increase in threat as the score increases.
        /// </summary>
        /// <value>
        /// The threat.
        /// </value>
        public float threat
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the last seen timestamp - when this cell was last seen by a unit of the AI.
        /// </summary>
        /// <value>
        /// The last seen.
        /// </value>
        public float lastSeen
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the list of entities currently residing inside this cell.
        /// </summary>
        /// <value>
        /// The entities.
        /// </value>
        public IList<IHasHealth> entities
        {
            get;
            private set;
        }
    }
}