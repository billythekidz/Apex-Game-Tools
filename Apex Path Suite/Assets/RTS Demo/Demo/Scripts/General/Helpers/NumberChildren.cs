#if UNITY_EDITOR

namespace Apex.Demo.RTS.AI
{
    using UnityEngine;

    /// <summary>
    /// Convenience component for applying numbering to child game objects in the Scene hierarchy. Should NOT be used in built games.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    [ExecuteInEditMode]
    public sealed class NumberChildren : MonoBehaviour
    {
        private void Awake()
        {
            var childCount = this.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                this.transform.GetChild(i).gameObject.name += " " + i;
            }
        }
    }
}

#endif