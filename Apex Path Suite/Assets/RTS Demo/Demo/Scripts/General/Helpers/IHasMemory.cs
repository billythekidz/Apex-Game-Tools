namespace Apex.Demo.RTS.AI
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents entities that have a memory, constituting two lists of observations
    /// </summary>
    public interface IHasMemory
    {
        /// <summary>
        /// Gets the current observations that make up the memory of this entity.
        /// </summary>
        /// <value>
        /// The observations.
        /// </value>
        IList<Observation> observations { get; }

        /// <summary>
        /// Gets the current enemy observations. Only includes killable enemies that exist in the memory of this entity.
        /// </summary>
        /// <value>
        /// The enemy observations.
        /// </value>
        IList<Observation> enemyObservations { get; }

        /// <summary>
        /// Adds the given entity as an observation. Creates a new observation for the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void AddObservation(IEntity entity);

        /// <summary>
        /// Adds the given observation to the AI's memory.
        /// </summary>
        /// <param name="observation"></param>
        void AddObservation(Observation observation);
    }
}