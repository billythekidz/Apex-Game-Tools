namespace Apex.Demo.RTS.AI
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Represents a 'setup object' for pooled structures, exposed through the Unity inspector for easy tweaking.
    /// </summary>
    [Serializable]
    public sealed class StructurePoolSetup
    {
        public StructureType type;
        public GameObject prefab;

        [Range(1, 100)]
        public int initialInstanceCount = 10;
    }
}