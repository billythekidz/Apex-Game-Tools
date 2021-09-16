namespace Apex.Demo.RTS.AI
{
    /// <summary>
    /// Represents a counter attack order. Issued to combat units when the AI controller is under attack and needs to defend its structures.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.OffensiveOrderBase" />
    public sealed class CounterAttackOrder : OffensiveOrderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CounterAttackOrder"/> class.
        /// </summary>
        /// <param name="priority">The priority.</param>
        /// <param name="target">The attack target.</param>
        public CounterAttackOrder(int priority, IUnit target)
            : base(priority, target)
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
            get { return OrderType.CounterAttack; }
        }
    }
}