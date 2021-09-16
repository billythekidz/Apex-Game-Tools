namespace Apex.Demo.RTS.AI
{
    using UnityEngine;

    /// <summary>
    /// Abstract base class representing pooled components.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    /// <seealso cref="Apex.Demo.RTS.AI.IPooled" />
    public abstract class PooledBase : MonoBehaviour, IPooled
    {
        [SerializeField, ReadOnly, Tooltip("The instance pool ID. Guaranteed to be unique.")]
        protected uint _poolId;

        /// <summary>
        /// Gets or sets the pooled identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public uint id
        {
            get { return _poolId; }
        }

        /// <summary>
        /// Called by Unity when this instance is enabled.
        /// </summary>
        protected virtual void OnEnable()
        {
            // entities get a unique pool id when enabled
            _poolId = Utils.nextPoolId;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="o">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object o)
        {
            var other = o as IPooled;
            if (other == null)
            {
                return false;
            }

            return other.id == _poolId;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return _poolId.GetHashCode();
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(PooledBase a, PooledBase b)
        {
            var oa = (object)a;
            var ob = (object)b;

            if (oa == null && ob == null)
            {
                return true;
            }

            if (oa == null || ob == null)
            {
                return false;
            }

            return a.id == b.id;
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(PooledBase a, PooledBase b)
        {
            return !(a == b);
        }
    }
}