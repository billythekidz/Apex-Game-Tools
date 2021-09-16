namespace Apex.Demo.RTS.AI
{
    using UnityEngine;

    /// <summary>
    /// Interface representing resource components for harvestable resources.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.IEntity" />
    public interface IResource : IEntity
    {
        /// <summary>
        /// Gets the type of the resource.
        /// </summary>
        /// <value>
        /// The type of the resource.
        /// </value>
        ResourceType resourceType { get; }

        /// <summary>
        /// Gets the current amount of resources.
        /// </summary>
        /// <value>
        /// The current resources.
        /// </value>
        int currentResources { get; }

        /// <summary>
        /// Gets the cached collider.
        /// </summary>
        /// <value>
        /// The cached collider.
        /// </value>
        Collider cachedCollider { get; }

        /// <summary>
        /// Gets the harvest positions - the positions from where workers may harvest this resource.
        /// </summary>
        /// <value>
        /// The harvest positions.
        /// </value>
        Vector3[] harvestPositions { get; }

        /// <summary>
        /// Gets the nearest harvest position compared to the given unit's position.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        Vector3 GetHarvestPosition(WorkerUnit unit);

        /// <summary>
        /// Harvests this resource for a variable amount of resources.
        /// </summary>
        /// <returns>The amount of resources harvested.</returns>
        int Harvest();
    }
}