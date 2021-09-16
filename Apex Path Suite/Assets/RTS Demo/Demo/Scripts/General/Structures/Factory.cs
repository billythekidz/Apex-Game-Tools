namespace Apex.Demo.RTS.AI
{
    /// <summary>
    /// Represents the Factory structure which can spawn combat units.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.SpawningStructureBase" />
    public sealed class Factory : SpawningStructureBase
    {
        /// <summary>
        /// Gets the type of the structure.
        /// </summary>
        /// <value>
        /// The type of the structure.
        /// </value>
        public override StructureType structureType
        {
            get { return StructureType.Factory; }
        }

        /// <summary>
        /// Spawns a unit of the specified type, if possible in regards to resources and whether this factory is already spawning a unit.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the unit was spawned, <c>false</c> otherwise.</returns>
        public bool SpawnUnit(UnitType type)
        {
            return HandleSpawnUnit(type);
        }
    }
}