namespace Apex.Demo.RTS.AI
{
    using UnityEngine;

    /// <summary>
    /// Interface for pooled instances.
    /// </summary>
    public interface IPooled
    {
        /// <summary>
        /// Gets or sets the pool identifier.
        /// </summary>
        /// <value>
        /// The pool identifier.
        /// </value>
        uint id { get; }

        /// <summary>
        /// Gets the game object.
        /// </summary>
        /// <value>
        /// The game object.
        /// </value>
        GameObject gameObject { get; }

        /// <summary>
        /// Gets the transform.
        /// </summary>
        /// <value>
        /// The transform.
        /// </value>
        Transform transform { get; }
    }
}