namespace Apex.Demo.RTS.AI
{
    /// <summary>
    /// Represents a ranged unit.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.UnitBase" />
    public sealed class RangedUnit : UnitBase
    {
        /// <summary>
        /// Gets the type of the unit.
        /// </summary>
        /// <value>
        /// The type of the unit.
        /// </value>
        public override UnitType unitType
        {
            get { return UnitType.Ranged; }
        }

        /// <summary>
        /// The internal attack method. For ranged units, adds a small muzzle particle effect to attacks.
        /// </summary>
        /// <param name="dmg">The DMG.</param>
        protected override void InternalAttack(float dmg)
        {
            base.InternalAttack(dmg);
            ParticlePoolManager.instance.SpawnParticleSystem(ParticlesType.SmallMuzzle, this.position);
        }
    }
}