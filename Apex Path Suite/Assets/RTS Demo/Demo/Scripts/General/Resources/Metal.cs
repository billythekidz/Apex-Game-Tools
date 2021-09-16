namespace Apex.Demo.RTS.AI
{
    /// <summary>
    /// Represents Metal resource components which may be harvested.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.ResourceBase" />
    public sealed class Metal : ResourceBase
    {
        /// <summary>
        /// Gets the type of the resource.
        /// </summary>
        /// <value>
        /// The type of the resource.
        /// </value>
        public override ResourceType resourceType
        {
            get { return ResourceType.Metal; }
        }
    }
}