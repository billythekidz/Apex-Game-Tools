namespace Apex.Demo.RTS.AI
{
    /// <summary>
    /// Represents a melee unit.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.UnitBase" />
    public sealed class MeleeUnit : UnitBase
    {
        /// <summary>
        /// Gets the type of the unit.
        /// </summary>
        /// <value>
        /// The type of the unit.
        /// </value>
        public override UnitType unitType
        {
            get { return UnitType.Melee; }
        }
    }
}