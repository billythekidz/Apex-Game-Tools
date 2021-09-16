namespace Apex.GettingStarted.AI
{
    using System;
    using Apex.AI;
    using Apex.AI.Components;
    using UnityEngine;

    public sealed class PositionContextProvider : MonoBehaviour, IContextProvider
    {
        private PositionContext _context;

        public void OnEnable()
        {
            _context = new PositionContext(this.gameObject);
        }

        public IAIContext GetContext(Guid aiId)
        {
            return _context;
        }
    }
}