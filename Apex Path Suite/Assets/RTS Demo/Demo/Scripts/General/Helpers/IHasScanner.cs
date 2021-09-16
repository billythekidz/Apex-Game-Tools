namespace Apex.Demo.RTS.AI
{
    /// <summary>
    /// Interface for entities that have scanning capabilities.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.IEntity" />
    public interface IHasScanner : IEntity
    {
        /// <summary>
        /// Gets the scan radius.
        /// </summary>
        /// <value>
        /// The scan radius.
        /// </value>
        float scanRadius { get; }
    }
}