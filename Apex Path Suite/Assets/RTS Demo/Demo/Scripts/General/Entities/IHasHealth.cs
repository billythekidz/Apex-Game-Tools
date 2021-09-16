namespace Apex.Demo.RTS.AI
{
    /// <summary>
    /// Interface for all entities that have health and therefore can be killed.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.IEntity" />
    public interface IHasHealth : IEntity
    {
        /// <summary>
        /// Gets the maximum health.
        /// </summary>
        /// <value>
        /// The maximum health.
        /// </value>
        float maxHealth { get; }

        /// <summary>
        /// Gets the current health.
        /// </summary>
        /// <value>
        /// The current health.
        /// </value>
        float currentHealth { get; }

        /// <summary>
        /// Gets the threat factor - how much threat this entity contributes to.
        /// </summary>
        /// <value>
        /// The threat factor.
        /// </value>
        float threatFactor { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is dead.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is dead; otherwise, <c>false</c>.
        /// </value>
        bool isDead { get; }

        /// <summary>
        /// Gets the last attacked timestamp.
        /// </summary>
        /// <value>
        /// The last attacked.
        /// </value>
        float lastAttacked { get; }

        /// <summary>
        /// Gets or sets the last attacker.
        /// </summary>
        /// <value>
        /// The last attacker.
        /// </value>
        IHasHealth lastAttacker { get; set; }

        /// <summary>
        /// Gets or sets the AI controller.
        /// </summary>
        /// <value>
        /// The controller.
        /// </value>
        AIController controller { get; set; }

        /// <summary>
        /// Receive the specified amount of damage.
        /// </summary>
        /// <param name="damage">The damage.</param>
        /// <returns>True if the entity is dead, false otherwise</returns>
        bool ReceiveDamage(float damage);

        /// <summary>
        /// Determines whether the specified other is allied.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        bool IsAlly(IHasHealth other);

        /// <summary>
        /// Determines whether the specified other is an enemy.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        bool IsEnemy(IHasHealth other);
    }
}