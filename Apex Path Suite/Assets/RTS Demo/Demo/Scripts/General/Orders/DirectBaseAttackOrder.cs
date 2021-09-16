namespace Apex.Demo.RTS.AI
{
    /// <summary>
    /// Represents a direct base attack order without attack or staging points. Used for groups that are already within the enemy's base.
    /// </summary>
    public sealed class DirectBaseAttackOrder : OffensiveGroupOrderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DirectBaseAttackOrder"/> class.
        /// </summary>
        /// <param name="priority">The priority.</param>
        /// <param name="target">The attack target.</param>
        public DirectBaseAttackOrder(int priority, IHasHealth target)
            : base(priority, target, target.position, null)
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
            get { return OrderType.DirectBaseAttack; }
        }
    }
}