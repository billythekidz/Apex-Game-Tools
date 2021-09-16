namespace Apex.Demo.RTS.AI
{
    using UnityEngine;

    /// <summary>
    /// Represents Energy Generators which generate energy for the AI at a slow, steady rate.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.StructureBase" />
    public sealed class EnergyGenerator : StructureBase
    {
        [Header("Energy Generator Only")]
        [SerializeField, Range(1, 10)]
        private int _energyPerSecond = 1;

        private float _lastEnergy;

        /// <summary>
        /// Gets the type of the structure.
        /// </summary>
        /// <value>
        /// The type of the structure.
        /// </value>
        public override StructureType structureType
        {
            get { return StructureType.EnergyGenerator; }
        }

        /// <summary>
        /// Gets the energy generated per second for this energy generator.
        /// </summary>
        /// <value>
        /// The energy per second.
        /// </value>
        public float energyPerSecond
        {
            get { return _energyPerSecond; }
        }

        /// <summary>
        /// Gets the name shown when the mouse hovers above.
        /// </summary>
        /// <value>
        /// The name when mouse hovers above.
        /// </value>
        public override string mouseOverName
        {
            get { return "Energy Generator"; }
        }

        /// <summary>
        /// Called by Unity once per frame.
        /// </summary>
        protected override void Update()
        {
            base.Update();

            var time = Time.timeSinceLevelLoad;
            if (time - _lastEnergy < 1f / _energyPerSecond)
            {
                return;
            }

            _lastEnergy = time;
            this.controller.AddResource(ResourceType.Energy, _energyPerSecond);
        }
    }
}