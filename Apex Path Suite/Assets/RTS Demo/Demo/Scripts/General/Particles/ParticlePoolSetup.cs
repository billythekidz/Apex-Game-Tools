namespace Apex.Demo.RTS.AI
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Represents a 'setup object' for pooled particle systems. Used for easy setup and tweaking of pooled particle system prefabs.
    /// </summary>
    [Serializable]
    public sealed class ParticlePoolSetup
    {
        public ParticlesType type;
        public GameObject prefab;

        [Range(1, 100)]
        public int initialInstanceCount = 25;
    }
}