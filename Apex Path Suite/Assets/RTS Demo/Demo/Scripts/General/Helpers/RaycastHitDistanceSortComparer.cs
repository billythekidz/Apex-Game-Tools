namespace Apex.Demo.RTS.AI
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// A comparer for sorting raycast hits based on distance to a supplied position.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IComparer{UnityEngine.RaycastHit}" />
    public class RaycastHitDistanceSortComparer : IComparer<RaycastHit>
    {
        private Vector3 _position;
        private int _sortDir;

        /// <summary>
        /// Initializes a new instance of the <see cref="RaycastHitDistanceSortComparer"/> class.
        /// </summary>
        /// <param name="ascending">if set to <c>true</c> sorts ascending.</param>
        public RaycastHitDistanceSortComparer(bool ascending)
            : this(Vector3.zero, ascending)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RaycastHitDistanceSortComparer"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="ascending">if set to <c>true</c> sorts ascending.</param>
        public RaycastHitDistanceSortComparer(Vector3 position, bool ascending)
        {
            _position = position;
            this.ascending = ascending;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RaycastHitDistanceSortComparer"/> is uses ascending sorting.
        /// </summary>
        /// <value>
        ///   <c>true</c> if ascending; otherwise, <c>false</c>.
        /// </value>
        public bool ascending
        {
            get { return _sortDir == 1; }
            set { _sortDir = value ? 1 : -1; }
        }

        /// <summary>
        /// Gets or sets the position used for the distance sorting.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Vector3 position
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        /// Compares the specified raycast hits.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public int Compare(RaycastHit a, RaycastHit b)
        {
            if (a.transform == null)
            {
                return 1 * _sortDir;
            }

            if (b.transform == null)
            {
                return -1 * _sortDir;
            }

            return (a.point - _position).sqrMagnitude.CompareTo((b.point - _position).sqrMagnitude) * _sortDir;
        }
    }
}