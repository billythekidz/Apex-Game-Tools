namespace Apex.GettingStarted.AI
{
    using Apex.AI;
    

    public sealed class NavMeshContext : IAIContext
    {
        public NavMeshContext(UnityEngine.AI.NavMeshAgent navMeshAgent)
        {
            this.navAgent = navMeshAgent;
        }

        /// <summary>
        /// Will never be null because the AINavMeshContextProvider has a required NavMeshAgent component
        /// </summary>
        public UnityEngine.AI.NavMeshAgent navAgent
        {
            get;
            private set;
        }
    }
}