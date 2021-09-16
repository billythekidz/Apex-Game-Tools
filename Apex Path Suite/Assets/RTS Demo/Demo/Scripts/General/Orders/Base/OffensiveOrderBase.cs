namespace Apex.Demo.RTS.AI
{
    /// <summary>
    /// Base class for all offensive orders - orders that involve attacking an attack target.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.OrderBase" />
    public abstract class OffensiveOrderBase : OrderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OffensiveOrderBase"/> class.
        /// </summary>
        /// <param name="priority">The priority.</param>
        /// <param name="target">The target.</param>
        public OffensiveOrderBase(int priority, IHasHealth target)
            : base(priority)
        {
            this.target = target;
        }

        /// <summary>
        /// Gets the order category.
        /// </summary>
        /// <value>
        /// The order category.
        /// </value>
        public override OrderCategoryType orderCategory
        {
            get { return OrderCategoryType.Offensive; }
        }

        /// <summary>
        /// Gets the attack target.
        /// </summary>
        /// <value>
        /// The target.
        /// </value>
        public IHasHealth target
        {
            get;
            private set;
        }
    }
}