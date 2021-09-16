namespace Apex.Demo.RTS.AI
{
    using UnityEngine;

    /// <summary>
    /// Component for facilitating easy use of layers in conjunction with physics (e.g. OverlapSphere).
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.SingletonMonoBehaviour{Apex.Demo.RTS.AI.LayersHelper}" />
    public sealed class LayersHelper : SingletonMonoBehaviour<LayersHelper>
    {
        public LayerMask unitLayer;
        public LayerMask resourceLayer;
        public LayerMask structureLayer;
        public LayerMask groundLayer;
    }
}