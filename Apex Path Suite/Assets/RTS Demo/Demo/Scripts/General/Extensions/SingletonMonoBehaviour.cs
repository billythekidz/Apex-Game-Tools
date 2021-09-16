namespace Apex.Demo.RTS.AI
{
    using UnityEngine;

    /// <summary>
    /// Base class for components that must behave as singletons - being unique in the scene and having a static instance property for easy access.
    /// </summary>
    /// <typeparam name="T">The type of the deriving component.</typeparam>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static T instance { get; private set; }

        /// <summary>
        /// Called by Unity when this instance awakes.
        /// </summary>
        protected virtual void Awake()
        {
            if (instance != null)
            {
                Debug.LogWarning(this.ToString() + " another instance has already been registered for this scene, destroying this one");
                Destroy(this);
                return;
            }

            instance = (T)this;
        }
    }
}