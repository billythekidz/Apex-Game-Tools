namespace Apex.Demo.RTS.AI
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Represents a 'setup object' for unit pools for easy tweaking in the Unity inspector.
    /// </summary>
    [Serializable]
    public sealed class UnitPoolSetup
    {
        public UnitType type;
        public GameObject prefab;

        [Range(1, 100)]
        public int initialInstanceCount = 20;
    }
}