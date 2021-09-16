namespace Apex.Demo.RTS.AI
{
    using UnityEngine;

    /// <summary>
    /// Represents a radar tower. It can scan and adds any observations to its controller's shared memory.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.AIStructureBase" />
    /// <seealso cref="Apex.Demo.RTS.AI.IHasScanner" />
    public sealed class RadarTower : AIStructureBase, IHasScanner
    {
        [Header("Radar Tower Only")]
        [SerializeField, Range(1f, 50f), Tooltip("The scanning radius for this radar tower.")]
        private float _scanRadius = 15f;

        /// <summary>
        /// Gets the type of the structure.
        /// </summary>
        /// <value>
        /// The type of the structure.
        /// </value>
        public override StructureType structureType
        {
            get { return StructureType.Radar; }
        }

        /// <summary>
        /// Gets the scan radius.
        /// </summary>
        /// <value>
        /// The scan radius.
        /// </value>
        public float scanRadius
        {
            get { return _scanRadius; }
        }
    }
}