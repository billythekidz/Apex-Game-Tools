namespace Apex.Demo.RTS.AI
{
    using UnityEngine;

    /// <summary>
    /// This class represents a worker's build target, which comprises of a structure type to build, and a position to build the structure at. The class is serializable for debugging purposes in the Unity editor, specifically to be able to show the build target in the editor.
    /// </summary>
    [System.Serializable]
    public sealed class BuildTarget
    {
        public StructureType type;
        public Vector3 position;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildTarget"/> class.
        /// </summary>
        /// <param name="type">The type of structure to build.</param>
        /// <param name="position">The position at which the structure should be built.</param>
        public BuildTarget(StructureType type, Vector3 position)
        {
            this.type = type;
            this.position = position;
        }
    }
}