namespace Apex.Demo.RTS.AI
{
    using UnityEngine;

    /// <summary>
    /// Represents a flanking attack order. Splits up the group in two and sends them each their own way in to the enemy's base, attacking from both flanks.
    /// </summary>
    public sealed class FlankingAttackOrder : OffensiveGroupOrderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FlankingAttackOrder"/> class.
        /// </summary>
        /// <param name="priority">The priority.</param>
        /// <param name="target">The attack target.</param>
        /// <param name="attackPoint">The attack point.</param>
        public FlankingAttackOrder(int priority, IHasHealth target, Vector3 attackPoint)
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
            get { return OrderType.FlankingAttack; }
        }
    }
}