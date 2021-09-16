namespace Apex.Demo.RTS.AI.Editor
{
    using UnityEngine;

    /// <summary>
    /// Visualizer for resources. Shows the harvest positions available. Purely for editor debug purposes. Uses Gizmos drawing.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    [RequireComponent(typeof(IResource))]
    public sealed class ResourceVisualizer : MonoBehaviour
    {
        [SerializeField, Range(0.1f, 2f)]
        private float positionSphereSize = 0.5f;

        [SerializeField]
        private Color gizmosColor = Color.green;

        private IResource _resource;

        private void OnEnable()
        {
            _resource = this.GetComponent<IResource>();
        }

        private void OnDrawGizmosSelected()
        {
            if (_resource == null)
            {
                return;
            }

            Gizmos.color = this.gizmosColor;
            var positions = _resource.harvestPositions;
            for (int i = 0; i < positions.Length; i++)
            {
                Gizmos.DrawSphere(positions[i], this.positionSphereSize);
            }
        }
    }
}