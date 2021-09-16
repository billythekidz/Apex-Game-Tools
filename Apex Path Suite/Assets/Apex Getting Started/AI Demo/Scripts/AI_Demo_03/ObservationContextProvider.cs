namespace Apex.GettingStarted.AI
{
    using System;
    using Apex.AI;
    using Apex.AI.Components;
    using UnityEngine;

    public sealed class ObservationContextProvider : MonoBehaviour, IContextProvider
    {
        private ObservationContext _context;

        public void OnEnable()
        {
            _context = new ObservationContext(this.gameObject);
        }

        public IAIContext GetContext(Guid aiId)
        {
            return _context;
        }
    }
}