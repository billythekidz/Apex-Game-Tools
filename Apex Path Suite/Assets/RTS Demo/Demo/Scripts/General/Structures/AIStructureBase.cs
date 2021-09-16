namespace Apex.Demo.RTS.AI
{
    using System;
    using Apex.AI;
    using Apex.AI.Components;

    /// <summary>
    /// Abstract base class for all structures that have a running AI.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.StructureBase" />
    /// <seealso cref="Apex.AI.Components.IContextProvider" />
    public abstract class AIStructureBase : StructureBase, IContextProvider
    {
        protected StructureContext _context;

        /// <summary>
        /// Called by Unity when enabled.
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            _context = new StructureContext(this);
        }

        /// <summary>
        /// Receive the specified amount of damage.
        /// </summary>
        /// <param name="damage">The damage.</param>
        /// <returns>
        /// <c>true</c> if the entity is dead, <c>false</c> otherwise
        /// </returns>
        public override bool ReceiveDamage(float damage)
        {
            var result = base.ReceiveDamage(damage);
            if (result)
            {
                // if this structure is dead, null the context object
                _context = null;
            }

            return result;
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