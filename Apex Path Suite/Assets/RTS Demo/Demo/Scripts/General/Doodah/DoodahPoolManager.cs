namespace Apex.Demo.RTS.AI
{
    using UnityEngine;

    /// <summary>
    /// Component for handling doodahs and their pool. This component is a singleton and will ensure that it is the only one in the scene.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.SingletonMonoBehaviour{Apex.Demo.RTS.AI.DoodahPoolManager}" />
    public sealed class DoodahPoolManager : SingletonMonoBehaviour<DoodahPoolManager>
    {
        [SerializeField, Tooltip("The prefab for doodahs. Will be pooled.")]
        private GameObject _doodahPrefab;

        [SerializeField, Range(1, 100), Tooltip("How many doodahs to instantiate from the beginning in the pool.")]
        private int _initialInstanceCount = 50;

        private DoodahPool _pool;
        private GameObject _host;

        protected override void Awake()
        {
            base.Awake();

            // setup pool
            _host = new GameObject("Doodahs");
            _host.transform.SetParent(this.transform);
            _pool = new DoodahPool(_doodahPrefab, _host, _initialInstanceCount);
        }

        /// <summary>
        /// Spawns the doodah and attaches it to the specified host.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <returns></returns>
        public IDoodah SpawnDoodah(Transform host)
        {
            var doodah = _pool.Get(host.position, host.rotation);
            doodah.transform.SetParent(host);
            return doodah;
        }

        /// <summary>
        /// Returns the specified doodah to the pool from whence it came.
        /// </summary>
        /// <param name="doodah">The doodah.</param>
        public void Return(IDoodah doodah)
        {
            doodah.transform.SetParent(_host.transform);
            _pool.Return(doodah);
        }
    }
}