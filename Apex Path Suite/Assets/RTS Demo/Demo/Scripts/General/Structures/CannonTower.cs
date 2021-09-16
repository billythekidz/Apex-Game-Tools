namespace Apex.Demo.RTS.AI
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Represents cannon tower structures, which can attack nearby enemies with high damage at a slow rate.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.AIStructureBase" />
    /// <seealso cref="Apex.Demo.RTS.AI.IHasScanner" />
    public sealed class CannonTower : AIStructureBase, IHasScanner, IHasAttack
    {
        [Header("Cannon Tower Only")]
        [SerializeField, Range(0f, 100f), Tooltip("The mimum damage inflicted by this cannon tower.")]
        private float _minDamage = 10f;

        [SerializeField, Range(1f, 200f), Tooltip("The maximum damage inflicted by this cannon tower.")]
        private float _maxDamage = 20f;

        [SerializeField, Range(0.001f, 10f), Tooltip("How many attacks per second this cannon may fire.")]
        private float _attacksPerSecond = 0.6f;

        [SerializeField, Range(1f, 100f), Tooltip("How far this cannon tower can see, its scanning radius.")]
        private float _scanRadius = 15f;

        [SerializeField, Range(1f, 100f), Tooltip("How far this cannon tower can attack, its attack radius.")]
        private float _attackRadius = 7.5f;

        [SerializeField, Tooltip("The turret script must be dragged into this field. Necessary for passing the attack target to the turret rotator script.")]
        private Turret _turret;

        [SerializeField, Tooltip("The transform where muzzle particle effects are spawned at. Allows for placing the particle system at the tip of the cannon barrel.")]
        private Transform _muzzleParticleAnchor;

        private float _lastAttack;

        /// <summary>
        /// Gets the type of the structure.
        /// </summary>
        /// <value>
        /// The type of the structure.
        /// </value>
        public override StructureType structureType
        {
            get { return StructureType.Cannon; }
        }

        /// <summary>
        /// Gets the minimum damage.
        /// </summary>
        /// <value>
        /// The minimum damage.
        /// </value>
        public float minDamage
        {
            get { return _minDamage; }
        }

        /// <summary>
        /// Gets the maximum damage.
        /// </summary>
        /// <value>
        /// The maximum damage.
        /// </value>
        public float maxDamage
        {
            get { return _maxDamage; }
        }

        /// <summary>
        /// Gets the attacks per second - how many attacks per second this cannon tower can fire.
        /// </summary>
        /// <value>
        /// The attacks per second.
        /// </value>
        public float attacksPerSecond
        {
            get { return _attacksPerSecond; }
        }

        /// <summary>
        /// Gets the scan radius.
        /// </summary>
        /// <value>
        /// The scan radius.
        /// </value>
        public float scanRadius
        {
            get { return _scanRadius; }
        }

        /// <summary>
        /// Gets the attack radius.
        /// </summary>
        /// <value>
        /// The attack radius.
        /// </value>
        public float attackRadius
        {
            get { return _attackRadius; }
        }

        /// <summary>
        /// Awakes this instance.
        /// </summary>
        /// <exception cref="ArgumentNullException">_turret</exception>
        private void Awake()
        {
            if (_turret == null)
            {
                _turret = this.GetComponentInChildren<Turret>();
                if (_turret == null)
                {
                    throw new ArgumentNullException("_turret");
                }
            }
        }

        /// <summary>
        /// Called by Unity once per frame.
        /// </summary>
        protected override void Update()
        {
            base.Update();

            if (_context == null)
            {
                return;
            }

            // set the contextual attack target as the turret script's target
            if (_context.attackTarget != null)
            {
                if (_turret.target != _context.attackTarget.transform)
                {
                    _turret.target = _context.attackTarget.transform;
                }
            }
            else if (_turret.target != null)
            {
                _turret.target = null;
            }
        }

        /// <summary>
        /// Gets a random damage between the set minimum and maximum damage for this cannon tower.
        /// </summary>
        /// <returns></returns>
        public float GetDamage()
        {
            return UnityEngine.Random.Range(_minDamage, _maxDamage);
        }

        /// <summary>
        /// Attacks the specified target - the cannon tower checks that the target is valid, and if so, turns and fires towards the target. Guaranteed to hit if target is valid.
        /// </summary>
        /// <param name="target">The target.</param>
        public void Attack(IHasHealth target)
        {
            var time = Time.timeSinceLevelLoad;
            if (time - _lastAttack < 1f / _attacksPerSecond)
            {
                return;
            }

            if ((target.transform.position - this.position).sqrMagnitude > (_attackRadius * _attackRadius))
            {
                // target too far away
                return;
            }

            if (IsAlly(target))
            {
                // hit is an ally, ignore
                return;
            }

            _lastAttack = time;
            target.lastAttacker = this;
            target.ReceiveDamage(GetDamage());

            var rot = Quaternion.LookRotation(_muzzleParticleAnchor.right);
            var upRot = Quaternion.LookRotation(Vector3.up);

            ParticlePoolManager.instance.SpawnParticleSystem(ParticlesType.SpentCartridgeCannon, this.position + Vector3.up + (_muzzleParticleAnchor.right * 2f), rot);
            ParticlePoolManager.instance.SpawnParticleSystem(ParticlesType.FlatImpactSmokeLarge, target.position + (Vector3.up * 0.5f), upRot);
            ParticlePoolManager.instance.SpawnParticleSystem(ParticlesType.VerticalImpactLarge, target.position, upRot);
            ParticlePoolManager.instance.SpawnParticleSystem(ParticlesType.BigMuzzleFire, _muzzleParticleAnchor.position, _muzzleParticleAnchor.rotation);
        }
    }
}