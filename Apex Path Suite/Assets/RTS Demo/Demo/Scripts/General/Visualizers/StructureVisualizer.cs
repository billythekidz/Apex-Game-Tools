namespace Apex.Demo.RTS.AI.Editor
{
    using UnityEngine;

    /// <summary>
    /// Visualizer for structures showing structure stats in the inspector. Purely for editor debug purposes.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    [RequireComponent(typeof(IStructure))]
    public class StructureVisualizer : MonoBehaviour
    {
        [Header("Controller")]
        [ReadOnly]
        public AIController controller;

        [Header("Health")]
        [ReadOnly]
        public bool isDead;

        [ReadOnly]
        public float currentHealth;

        [ReadOnly]
        public float healthPercentage;

        [Header("Ready")]
        [ReadOnly]
        public bool isReady;

        [Header("Threat")]
        [ReadOnly]
        public float threatScore;

        private StructureBase _structure;

        protected virtual void OnEnable()
        {
            _structure = this.GetComponent<StructureBase>();
        }

        protected virtual void Update()
        {
            if (_structure == null)
            {
                return;
            }

            this.controller = _structure.controller;

            this.isDead = _structure.isDead;
            this.currentHealth = _structure.currentHealth;
            this.healthPercentage = (this.currentHealth / _structure.maxHealth) * 100f;

            this.isReady = _structure.isReady;

            this.threatScore = _structure.threatScore;
        }
    }
}