namespace Apex.Demo.RTS.AI
{
    using UnityEngine;

    /// <summary>
    /// Base interface for all entities in the RTS Demo
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.IPooled" />
    public interface IEntity : IPooled
    {
        /// <summary>
        /// Gets the type of the entity.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        EntityType entityType { get; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        Vector3 position { get; set; }

        /// <summary>
        /// Gets or sets the rotation.
        /// </summary>
        /// <value>
        /// The rotation.
        /// </value>
        Quaternion rotation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IEntity"/> is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if active; otherwise, <c>false</c>.
        /// </value>
        bool active { get; set; }
    }
}