namespace Apex.Demo.RTS.AI
{
    using UnityEngine;

    /// <summary>
    /// Represents frontal attacks on enemy's base. The group moves frontally into the enemy's base through a single attack point.
    /// </summary>
    public sealed class FrontalAttackOrder : OffensiveGroupOrderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FrontalAttackOrder"/> class.
        /// </summary>
        /// <param name="priority">The priority.</param>
        /// <param name="target">The attack target.</param>
        /// <param name="attackPoint">The attack point.</param>
        public FrontalAttackOrder(int priority, IHasHealth target, Vector3 attackPoint)
            : base(priority, target, attackPoint, null)
        {
        }

        /// <summary>
        /// Gets the specific type of order.
        /// </summary>
        /// <value>
        /// The type of the order.
        /// </value>
        public override OrderType orderType
        {
            get { return OrderType.FrontalAttack; }
        }
    }
}