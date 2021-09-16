namespace Apex.Demo.RTS.AI
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// A component for facilitating easy management of health bars.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.SingletonMonoBehaviour{Apex.Demo.RTS.AI.HealthbarManager}" />
    public sealed class HealthbarManager : SingletonMonoBehaviour<HealthbarManager>
    {
        [SerializeField]
        private GameObject _healthbarPrefab;

        private void OnEnable()
        {
            if (_healthbarPrefab == null)
            {
                throw new ArgumentNullException("_healthbarPrefab");
            }
        }

        /// <summary>
        /// Instantiates a healthbar game object and returns the Slider component found on the healthbar.
        /// </summary>
        /// <returns></returns>
        public Slider GetHealthbar()
        {
            var go = (GameObject)Instantiate(_healthbarPrefab, Vector3.zero, Quaternion.identity);
            go.transform.SetParent(this.transform); // all healthbars reside as children of the Canvas object (this game object)
            go.transform.localScale = Vector3.one; // we must set the local scale, otherwise the healthbars do not show up
            return go.GetComponentInChildren<Slider>();
        }
    }
}