namespace Apex.Demo.RTS.AI
{
    using UnityEngine;

    /// <summary>
    /// Abstract base class for all entities in the RTS demo project.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.PooledBase" />
    /// <seealso cref="Apex.Demo.RTS.AI.IEntity" />
    [RequireComponent(typeof(Collider))]
    public abstract class EntityBase : PooledBase, IEntity
    {
        /// <summary>
        /// Gets the type of the entity.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        public abstract EntityType entityType { get; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Vector3 position
        {
            get { return this.transform.position; }
            set { this.transform.position = value; }
        }

        /// <summary>
        /// Gets or sets the rotation.
        /// </summary>
        /// <value>
        /// The rotation.
        /// </value>
        public Quaternion rotation
        {
            get { return this.transform.rotation; }
            set { this.transform.rotation = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="EntityBase"/> is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if active; otherwise, <c>false</c>.
        /// </value>
        public bool active
        {
            get { return this.gameObject.activeSelf; }
            set { this.gameObject.SetActive(value); }
        }

        /// <summary>
        /// Gets the name when mouse hovers above.
        /// </summary>
        /// <value>
        /// The name when mouse hovers above.
        /// </value>
        public abstract string mouseOverName { get; }

        /// <summary>
        /// Called by Unity when enabled.
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();

            this.Link(this.GetComponent<Collider>());
        }

        /// <summary>
        /// Called by Unity when disabled.
        /// </summary>
        protected virtual void OnDisable()
        {
            this.Unlink(this.GetComponent<Collider>());
        }

        /// <summary>
        /// Called by Unity when [mouse over].
        /// </summary>
        private void OnMouseOver()
        {
            MouseOverManager.instance.ShowMouseOver(this.mouseOverName, this.transform.position);
        }

        /// <summary>
        /// Called by Unity when [mouse exit].
        /// </summary>
        private void OnMouseExit()
        {
            MouseOverManager.instance.HideMouseOver();
        }
    }
}