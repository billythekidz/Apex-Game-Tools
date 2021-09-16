namespace Apex.Demo.RTS.AI
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Abstract base class component for all entities that have health.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.EntityBase" />
    /// <seealso cref="Apex.Demo.RTS.AI.IHasHealth" />
    public abstract class HasHealthBase : EntityBase, IHasHealth
    {
        [Header("Controller")]
        [SerializeField, Tooltip("The AI controller 'owning' this entity. Can be manually assigned for entities that start in the scene, is set automatically otherwise.")]
        private AIController _controller;

        [Header("Health")]
        [SerializeField, Range(10f, 1000f), Tooltip("The maximum health for this entity, the health that this unit starts with.")]
        protected float _maxHealth = 100f;

        [SerializeField, Range(1f, 10f), Tooltip("The Y position modifier for healthbars. They are placed this amount of units in the Y-axis above this entity.")]
        private float _healthBarY = 5f;

        [Header("Death")]
        [SerializeField, ReadOnly, Tooltip("The most recent other entity which attacked this one.")]
        protected IHasHealth _lastAttacker;

        [SerializeField, ReadOnly, Tooltip("The timestamp in seconds since level load for the most recent attack on this entity.")]
        private float _lastAttacked;

        [Header("Threat")]
        [SerializeField, Range(0.01f, 1000f), Tooltip("How much threat this entity contributes to when calculating threat-based influence maps.")]
        private float _threatFactor = 10f;

        private Slider _healthBar;

        /// <summary>
        /// Gets the maximum health.
        /// </summary>
        /// <value>
        /// The maximum health.
        /// </value>
        public float maxHealth
        {
            get { return _maxHealth; }
        }

        /// <summary>
        /// Gets the current health.
        /// </summary>
        /// <value>
        /// The current health.
        /// </value>
        public float currentHealth
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the threat factor - how much threat this entity contributes to influence maps by.
        /// </summary>
        /// <value>
        /// The threat factor.
        /// </value>
        public float threatFactor
        {
            get { return _threatFactor; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is dead.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is dead; otherwise, <c>false</c>.
        /// </value>
        public bool isDead
        {
            get { return this.currentHealth <= 0f || (this.gameObject != null && !this.active); }
        }

        /// <summary>
        /// Gets the last attacked timestamp.
        /// </summary>
        /// <value>
        /// The last attacked.
        /// </value>
        public float lastAttacked
        {
            get { return _lastAttacked; }
        }

        /// <summary>
        /// Gets or sets the last attacker.
        /// </summary>
        /// <value>
        /// The last attacker.
        /// </value>
        public IHasHealth lastAttacker
        {
            get
            {
                return _lastAttacker;
            }

            set
            {
                _lastAttacker = value;
                _lastAttacked = Time.timeSinceLevelLoad;
            }
        }

        /// <summary>
        /// Gets or sets the AI controller.
        /// </summary>
        /// <value>
        /// The controller.
        /// </value>
        public AIController controller
        {
            get { return _controller; }
            set { _controller = value; }
        }

        /// <summary>
        /// Called by Unity when enabled.
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();

            // always max current health when being enabled
            this.currentHealth = _maxHealth;

            if (_healthBar == null)
            {
                // if this entity does not already have a healthbar, make sure that it gets one
                _healthBar = HealthbarManager.instance.GetHealthbar();
            }

            // enable the healthbar when this object is enabled
            _healthBar.gameObject.SetActive(true);

            // set the color on enable if the entity already has been assigned a controller through the Inspector
            if (_controller != null)
            {
                this.gameObject.ColorRenderers(controller.color);
            }
        }

        /// <summary>
        /// Called by Unity when disabled.
        /// </summary>
        protected override void OnDisable()
        {
            base.OnDisable();

            // disable the healthbar when this object is disabled
            if (_healthBar != null)
            {
                _healthBar.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Called by Unity once per frame.
        /// </summary>
        protected virtual void Update()
        {
            if (_healthBar == null || !_healthBar.gameObject.activeSelf || Camera.main == null)
            {
                // only update the healthbar if it is active and available
                return;
            }

            // project 3D world position to viewport position
            var pos = Camera.main.WorldToViewportPoint(this.position + new Vector3(0f, _healthBarY, 0f));

            // the projected position is in the range of [-1, -1] to [1, 1] so in order to convert it to pixel space, we multiply with the screen size
            _healthBar.transform.position = new Vector3(pos.x * Screen.width, pos.y * Screen.height, 0f);

            // update the value of the healthbar slider
            _healthBar.value = this.currentHealth / _maxHealth;
        }

        /// <summary>
        /// Receive the specified amount of damage.
        /// </summary>
        /// <param name="damage">The damage.</param>
        /// <returns>
        /// True if the entity is dead, false otherwise
        /// </returns>
        public abstract bool ReceiveDamage(float damage);

        /// <summary>
        /// Determines whether the specified other is allied.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual bool IsAlly(IHasHealth other)
        {
            return ReferenceEquals(this.controller, other.controller);
        }

        /// <summary>
        /// Determines whether the specified other is an enemy.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual bool IsEnemy(IHasHealth other)
        {
            return !IsAlly(other);
        }
    }
}