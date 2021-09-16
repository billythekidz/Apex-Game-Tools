namespace Apex.Demo.RTS.AI
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Represents an attack order which seeks to exploit the weakest point in the enemy's defense. Uses an attack point and a staging point to approach from the weakest (least threatening) position.
    /// </summary>
    public sealed class WeakestPointAttackOrder : OffensiveGroupOrderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WeakestPointAttackOrder"/> class.
        /// </summary>
        /// <param name="priority">The priority.</param>
        /// <param name="target">The attack target.</param>
        /// <param name="attackPoint">The attack point.</param>
        /// <param name="stagingPoints">The staging points.</param>
        public WeakestPointAttackOrder(int priority, IHasHealth target, Vector3 attackPoint, IList<Vector3> stagingPoints)
            : base(priority, target, attackPoint, stagingPoints)
        {
        }

        /// <summary>
        /// The specific type of order.
        /// </summary>
        public override OrderType orderType
        {
            get { return OrderType.WeakestPointAttack; }
        }
    }
}