namespace Apex.Demo.RTS.AI
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Simple static extension class providing convenient extension methods.
    /// </summary>
    public static class SimpleExtensions
    {
        /// <summary>
        /// Colors all renderers on a given game object with the same tint. Can optionally avoid coloring particle systems.
        /// </summary>
        /// <param name="go">The game object.</param>
        /// <param name="color">The color.</param>
        /// <param name="ignoreParticleSystems">if set to <c>true</c>, ignores particle systems and thus avoids coloring them.</param>
        public static void ColorRenderers(this GameObject go, Color color, bool ignoreParticleSystems = true)
        {
            // first iterate through all renderes on the game object itself
            var renderers = go.GetComponents<Renderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                if (!ignoreParticleSystems || renderers[i].GetComponent<ParticleSystem>() == null)
                {
                    renderers[i].material.color = color;
                }
            }

            // then iterate through all renderers in child game objects
            renderers = go.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                if (!ignoreParticleSystems || renderers[i].GetComponent<ParticleSystem>() == null)
                {
                    renderers[i].material.color = color;
                }
            }
        }

        /// <summary>
        /// Gets the observed enemies as a specific type. Used to make it easy to get all enemy observations as a specific entity type, e.g. units or structures.
        /// </summary>
        /// <typeparam name="T">The desired type of entity.</typeparam>
        /// <param name="controller">The controller.</param>
        /// <param name="buffer">The buffer to populate with the resulting elements.</param>
        public static void GetObservedEnemies<T>(this AIController controller, IList<T> buffer) where T : class, IHasHealth
        {
            var observations = controller.enemyObservations;
            var count = observations.Count;
            for (int i = 0; i < count; i++)
            {
                var candidate = observations[i].GetEntity<T>();
                if (candidate == null)
                {
                    continue;
                }

                buffer.Add(candidate);
            }
        }

        /// <summary>
        /// Gets the dictionary value or the default value.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dict">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default(TValue))
        {
            TValue val = defaultValue;
            dict.TryGetValue(key, out val);
            return val;
        }
    }
}