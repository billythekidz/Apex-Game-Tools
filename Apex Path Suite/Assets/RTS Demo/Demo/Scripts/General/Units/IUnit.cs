namespace Apex.Demo.RTS.AI
{
    using Apex.AI.Components;
    using UnityEngine;

    /// <summary>
    /// Represents all units. Units are movable entities that have health and can attack. They also have memories and scanners, and can receive orders, which makes up the basis for their AI.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.IHasHealth" />
    /// <seealso cref="Apex.Demo.RTS.AI.IHasScanner" />
    /// <seealso cref="Apex.Demo.RTS.AI.IHasOrders" />
    public interface IUnit : IHasHealth, IHasAttack, IHasMemory, IHasScanner, IHasOrders, IContextProvider
    {
        /// <summary>
        /// Gets the type of the unit.
        /// </summary>
        /// <value>
        /// The type of the unit.
        /// </value>
        UnitType unitType { get; }

        /// <summary>
        /// Gets a value indicating whether this unit is moving.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is moving; otherwise, <c>false</c>.
        /// </value>
        bool isMoving { get; }

        /// <summary>
        /// Gets a value indicating whether this unit is idle - that is, not executing an order, not moving and not dead.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is idle; otherwise, <c>false</c>.
        /// </value>
        bool isIdle { get; }

        /// <summary>
        /// Gets the NavMesh area mask.
        /// </summary>
        /// <value>
        /// The area mask.
        /// </value>
        LayerMask areaMask { get; }

        /// <summary>
        /// Gets or sets the unit group that this unit belongs to. Can be null (e.g. for Workers).
        /// </summary>
        /// <value>
        /// The group.
        /// </value>
        UnitGroup group { get; set; }

        /// <summary>
        /// Makes this unit wander randomly to a new destination.
        /// </summary>
        void RandomWander();

        /// <summary>
        /// Move this unit to the given destination.
        /// </summary>
        /// <param name="destination">The destination.</param>
        void MoveTo(Vector3 destination);

        /// <summary>
        /// Stops all movement.
        /// </summary>
        void StopMoving();

        /// <summary>
        /// Turns this unit to face the given position.
        /// </summary>
        /// <param name="pos">The position.</param>
        void LookAt(Vector3 pos);
    }
}