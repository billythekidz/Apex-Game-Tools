namespace Apex.Demo.RTS.AI
{
    public interface IHasAttack : IHasHealth
    {
        /// <summary>
        /// Gets the minimum damage that this entity can inflict.
        /// </summary>
        /// <value>
        /// The minimum damage.
        /// </value>
        float minDamage { get; }

        /// <summary>
        /// Gets the maximum damage that this entity can inflict.
        /// </summary>
        /// <value>
        /// The maximum damage.
        /// </value>
        float maxDamage { get; }

        /// <summary>
        /// Gets the amount of attacks per second that this entity can execute.
        /// </summary>
        /// <value>
        /// The attacks per second.
        /// </value>
        float attacksPerSecond { get; }

        /// <summary>
        /// Gets the attack radius - the radius at which this entity may attack others.
        /// </summary>
        /// <value>
        /// The attack radius.
        /// </value>
        float attackRadius { get; }

        /// <summary>
        /// Gets a random amouunt of damage between the set minimum and maximum damage.
        /// </summary>
        /// <returns></returns>
        float GetDamage();

        /// <summary>
        /// Turn to face and attack the specified target, if the target is valid.
        /// </summary>
        /// <param name="target">The target.</param>
        void Attack(IHasHealth target);
    }
}