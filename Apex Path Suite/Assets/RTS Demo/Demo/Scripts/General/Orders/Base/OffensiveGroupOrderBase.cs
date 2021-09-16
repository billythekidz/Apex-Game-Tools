namespace Apex.Demo.RTS.AI
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Base class for all offensive orders issued to groups. 
    /// Groups' offensive orders include an attack point and staging points.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.OffensiveOrderBase" />
    public abstract class OffensiveGroupOrderBase : OffensiveOrderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OffensiveGroupOrderBase"/> class.
        /// </summary>
        /// <param name="priority">The priority.</param>
        /// <param name="target">The attack target.</param>
        /// <param name="attackPoint">The attack point.</param>
        /// <param name="stagingPoints">The staging points.</param>
        public OffensiveGroupOrderBase(int priority, IHasHealth target, Vector3 attackPoint, IList<Vector3> stagingPoints = null)
            : base(priority, target)
        {
            this.attackPoint = attackPoint;
            this.stagingPoints = stagingPoints;
        }

        /// <summary>
        /// Gets the attack point - the point of entry for attacking.
        /// </summary>
        /// <value>
        /// The attack point.
        /// </value>
        public Vector3 attackPoint
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the staging points - a list of waypoints .
        /// </summary>
        /// <value>
        /// The staging points.
        /// </value>
        public IList<Vector3> stagingPoints
        {
            get;
            private set;
        }
    }
}