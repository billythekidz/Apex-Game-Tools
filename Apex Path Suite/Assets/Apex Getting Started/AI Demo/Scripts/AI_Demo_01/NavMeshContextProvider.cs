namespace Apex.GettingStarted.AI
{
    using System;
    using Apex.AI;
    using Apex.AI.Components;
    using UnityEngine;


    // We're adding a required component of type NavMeshAgen, which means that the game object requires a NavMeshAgent component
    [RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
    public sealed class NavMeshContextProvider : MonoBehaviour, IContextProvider
    {
        private NavMeshContext _context;

        private void OnEnable()
        {
            // Creating the NavMeshContext so we can return it to the scorers and actions
            _context = new NavMeshContext(this.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>());
        }

        public IAIContext GetContext(Guid aiId)
        {
            return _context;
        }
    }
}