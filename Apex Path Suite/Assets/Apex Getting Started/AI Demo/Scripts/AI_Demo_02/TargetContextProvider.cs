namespace Apex.GettingStarted.AI
{
    using System;
    using Apex.AI;
    using Apex.AI.Components;
    using UnityEngine;

    public sealed class TargetContextProvider : MonoBehaviour, IContextProvider
    {
        [SerializeField]
        private GameObject[] _targets = new GameObject[0];

        private TargetContext _context;

        public void OnEnable()
        {
            _context = new TargetContext(this.transform, _targets);
        }

        public IAIContext GetContext(Guid aiId)
        {
            return _context;
        }
    }
}