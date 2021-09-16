namespace Apex.Demo.RTS.AI
{
    /// <summary>
    /// Represents all structures. All structures are killable entities with health.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.IHasHealth" />
    public interface IStructure : IHasHealth
    {
        /// <summary>
        /// Gets the type of the structure.
        /// </summary>
        /// <value>
        /// The type of the structure.
        /// </value>
        StructureType structureType { get; }

        /// <summary>
        /// Gets or sets the threat score - how threatened this structure is analyzed to be in the eyes of the owning AI controller.
        /// </summary>
        /// <value>
        /// The threat score.
        /// </value>
        float threatScore { get; set; }

        /// <summary>
        /// Gets the threat priority - how prioritized this structure is set to be.
        /// </summary>
        /// <value>
        /// The threat priority.
        /// </value>
        float threatPriority { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is ready, e.g. to attack or spawn units.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is ready; otherwise, <c>false</c>.
        /// </value>
        bool isReady { get; set; }
    }
}