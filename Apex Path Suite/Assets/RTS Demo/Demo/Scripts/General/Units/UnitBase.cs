namespace Apex.Demo.RTS.AI
{
    using System;
    using System.Collections.Generic;
    using Apex.AI;
    using UnityEngine;


    /// <summary>
    /// Abstract base class for all units.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.HasHealthBase" />
    /// <seealso cref="Apex.Demo.RTS.AI.IUnit" />
    [RequireComponent(typeof(Rigidbody), typeof(UnityEngine.AI.NavMeshAgent))]
    public abstract class UnitBase : HasHealthBase, IUnit
    {
        [Header("Movement")]
        [SerializeField, Range(0.0001f, 0.9999f)]
        protected float _minMovingThreshold = 0.01f;

        [Header("Attacking")]
        [SerializeField, Range(0f, 100f)]
        protected float _minDamage = 10f;

        [SerializeField, Range(1f, 200f)]
        protected float _maxDamage = 20f;

        [SerializeField, Range(0.001f, 10f)]
        protected float _attacksPerSecond = 0.6f;

        [Header("Radii")]
        [SerializeField, Range(1f, 50f)]
        protected float _scanRadius = 15f;

        [SerializeField, Range(1f, 50f)]
        protected float _attackRadius = 7.5f;

        [SerializeField, Range(2f, 50f)]
        protected float _randomWanderRadius = 20f;

        [SerializeField, Range(0.1f, 10f)]
        private float _allowedMovementImprecision = 1f;

        protected readonly RaycastHitDistanceSortComparer _comparer = new RaycastHitDistanceSortComparer(true);
        protected float _unitRadius;
        protected Animator _animator;

        private UnityEngine.AI.NavMeshAgent _agent;
        private float _lastAttack;
        private UnitContext _context;
        private Vector3 _velocity;
        private Vector3 _lastPos;

        public abstract UnitType unitType { get; }

        public override EntityType entityType { get { return EntityType.Unit; } }

        public UnitContext context
        {
            get { return _context; }
        }

        public float minDamage
        {
            get { return _minDamage; }
        }

        public float maxDamage
        {
            get { return _maxDamage; }
        }

        public float attacksPerSecond
        {
            get { return _attacksPerSecond; }
        }

        public float scanRadius
        {
            get { return _scanRadius; }
        }

        public float attackRadius
        {
            get { return _attackRadius; }
        }

        public bool isMoving
        {
            get { return _velocity.sqrMagnitude > (_minMovingThreshold * _minMovingThreshold); }
        }

        public virtual bool isIdle
        {
            get { return !this.isMoving && !this.isDead && this.currentlyExecuting == null; }
        }

        public LayerMask areaMask
        {
            get { return _agent.areaMask; }
        }

        public IList<IOrder> orders
        {
            get;
            private set;
        }

        public IList<IOrder> completedOrders
        {
            get;
            private set;
        }

        public IOrder currentlyExecuting
        {
            get;
            set;
        }

        public IOrder currentOrder
        {
            get;
            set;
        }

        public UnitGroup group
        {
            get;
            set;
        }

        public IList<Observation> observations
        {
            get;
            private set;
        }

        public IList<Observation> enemyObservations
        {
            get;
            private set;
        }

        public override string mouseOverName
        {
            get { return this.unitType.ToString(); }
        }

        protected virtual void Awake()
        {
            _animator = this.GetComponentInChildren<Animator>();
            if (_animator == null)
            {
                throw new ArgumentNullException("_animator");
            }

            _agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();

            // we give the unit a random avoidance priority so as to ensure that units will actually avoid each other (since same priority units will not try to avoid each other)
            _agent.avoidancePriority = UnityEngine.Random.Range(0, 99);

            // units require a capsule or sphere collider
            var capsule = this.GetComponent<CapsuleCollider>();
            _unitRadius = capsule != null ? capsule.radius : this.GetComponent<SphereCollider>().radius;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _context = new UnitContext(this);

            this.orders = new List<IOrder>(10); // TODO: better preallocation
            this.completedOrders = new List<IOrder>(10);

            this.observations = new List<Observation>(50);
            this.enemyObservations = new List<Observation>(25);

            _lastPos = this.transform.position;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (this.group != null)
            {
                this.group.Remove(this);
                this.group = null;
            }

            _context = null;
            this.orders = null;
            this.completedOrders = null;
            this.currentOrder = null;
            this.currentlyExecuting = null;

            this.observations = null;
            this.enemyObservations = null;
        }

        protected override void Update()
        {
            base.Update();

            if (_context == null)
            {
                return;
            }

            _velocity = (this.position - _lastPos).OnlyXZ();
            _lastPos = this.position;

            _animator.SetBool("IsWalking", this.isMoving);
        }

        public void RandomWander()
        {
            var pos = this.transform.position + (UnityEngine.Random.onUnitSphere.normalized * _randomWanderRadius);
            pos.y = this.position.y;
            MoveTo(pos);
        }

        public void MoveTo(Vector3 destination)
        {
            UnityEngine.AI.NavMeshHit hit;
            if (UnityEngine.AI.NavMesh.SamplePosition(destination, out hit, _allowedMovementImprecision, this.areaMask))
            {
                if ((hit.position - this.position).sqrMagnitude < (_agent.stoppingDistance * _agent.stoppingDistance))
                {
                    // destination not far enough away
                    return;
                }

                _agent.Resume();
                _agent.SetDestination(hit.position);
            }
        }

        public void StopMoving()
        {
            _agent.Stop();
            _agent.ResetPath();
        }

        public void LookAt(Vector3 pos)
        {
            this.transform.LookAt(new Vector3(pos.x, this.transform.position.y, pos.z), Vector3.up);
        }

        /// <summary>
        /// Adds the given entity as an observation. Creates a new observation for the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void AddObservation(IEntity entity)
        {
            AddObservation(new Observation(entity));
        }

        /// <summary>
        /// Adds the given observation to the AI's memory.
        /// </summary>
        /// <param name="observation"></param>
        public void AddObservation(Observation observation)
        {
            AddOrReplaceIfNewer(this.observations, observation);

            var hasHealthObs = observation.GetEntity<IHasHealth>();
            if (hasHealthObs == null || IsAlly(hasHealthObs))
            {
                // non-killables and allies are not enemies
                return;
            }

            // add to enemy observations list
            AddOrReplaceIfNewer(this.enemyObservations, observation);
        }

        /// <summary>
        /// Adds the given observation to the supplied list, if either the entity has not been observed previously, or if the observation is newer than the previous observation.
        /// </summary>
        /// <param name="list">The list to add the new observation to.</param>
        /// <param name="observation">The new observation.</param>
        private void AddOrReplaceIfNewer(IList<Observation> list, Observation observation)
        {
            var count = list.Count;
            for (int i = 0; i < count; i++)
            {
                var obs = list[i];
                if (obs.entity.id != observation.entity.id)
                {
                    // not same entity
                    continue;
                }

                if (observation.timestamp < obs.timestamp)
                {
                    // only replace if the incoming timestamp is newer
                    continue;
                }

                list[i] = observation;
                return;
            }

            // observation did not already exist, so add as new one
            list.Add(observation);
        }

        /// <summary>
        /// Gets a random amouunt of damage between the set minimum and maximum damage.
        /// </summary>
        /// <returns></returns>
        public float GetDamage()
        {
            return UnityEngine.Random.Range(_minDamage, _maxDamage);
        }

        /// <summary>
        /// Turn to face and attack the specified target, if the target is valid.
        /// </summary>
        /// <param name="target">The target.</param>
        public void Attack(IHasHealth target)
        {
            var time = Time.timeSinceLevelLoad;
            if (time - _lastAttack < 1f / _attacksPerSecond)
            {
                return;
            }

            _lastAttack = time;
            this.LookAt(target.position);
            StopMoving();

            _animator.SetTrigger("Shoot");
            InternalAttack(GetDamage());
        }

        /// <summary>
        /// Receives the given damage. Performs cleanup, plays particle systems and returns the unit to its unit pool.
        /// </summary>
        /// <param name="damage">The damage.</param>
        /// <returns></returns>
        public override bool ReceiveDamage(float damage)
        {
            if (this.isDead)
            {
                return true;
            }

            this.currentHealth -= damage;

            if (this.currentHealth <= 0f)
            {
                // only do this if the controller is not null, indicating that it has been in-game (as opposed to at start-up)
                if (this.controller != null)
                {
                    this.controller.units.Remove(this);
                }

                if (this.group != null)
                {
                    this.group.Remove(this);
                }

                _context = null;

                UnitPoolManager.instance.Return(this);

                DecalPoolManager.instance.SpawnDecal(DecalType.ExplosionSmall, this.position);
                ParticlePoolManager.instance.SpawnParticleSystem(ParticlesType.ImpactSmokeSmall, this.position + (Vector3.up * 0.5f));
                ParticlePoolManager.instance.SpawnParticleSystem(ParticlesType.Sparks, this.position);

                return true;
            }
            else
            {
                _animator.SetTrigger("Hit");
            }

            return false;
        }

        /// <summary>
        /// The internal attack method which actually executes the attack. Uses sphere cast to determine whether it hits the attack target.
        /// </summary>
        /// <param name="dmg">The DMG.</param>
        protected virtual void InternalAttack(float dmg)
        {
            var layers = LayersHelper.instance;
            var hits = Utils.hitsBuffer;
            var pos = this.position + this.transform.forward * _unitRadius;
            Physics.SphereCastNonAlloc(pos, _unitRadius * 2f, this.transform.forward, hits, _attackRadius, layers.unitLayer | layers.structureLayer);

            // Sort the results to ensure that the closest hit is actually damaged
            _comparer.position = this.position;
            Array.Sort(hits, _comparer);

            for (int i = 0; i < hits.Length; i++)
            {
                var hit = hits[i];
                if (hit.transform == null)
                {
                    continue;
                }

                if (hit.transform == this.transform)
                {
                    // ignore hits with self
                    continue;
                }

                var hasHealth = hit.collider.GetEntity<IHasHealth>();
                if (hasHealth == null || hasHealth.isDead)
                {
                    // hit is invalid or dead - ignore
                    continue;
                }

                if (IsAlly(hasHealth))
                {
                    // hit is an ally, ignore
                    continue;
                }

                // apply damage to single (first) hit
                hasHealth.lastAttacker = this;
                hasHealth.ReceiveDamage(dmg);
                break;
            }
        }

        /// <summary>
        /// Retrieves the context instance. This can be a simple getter or a factory method.
        /// </summary>
        /// <param name="aiId">The Id of the requesting AI.</param>
        /// <returns>
        /// The concrete context instance for use by the requester.
        /// </returns>
        public IAIContext GetContext(Guid aiId)
        {
            return _context;
        }
    }
}