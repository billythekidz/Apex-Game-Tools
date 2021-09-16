namespace Apex.GettingStarted.AI
{
    using Apex.AI;
    using Serialization;
    using UnityEngine;


    /// <summary>
    /// An Action that will make the nav agent move to a random point
    /// </summary>
    public sealed class RandomMove : ActionBase<NavMeshContext>   //The Type later used in the Execute method as a parameter
    {
        // The maximum radius in units the agent will move
        [ApexSerialization(defaultValue = 5f), FriendlyName("Move Radius", "The distance at which random positions are generated at")]
        public float moveRadius = 5f;

        [ApexSerialization(defaultValue = 1f), FriendlyName("Sampling Threshold", "Random point sampling threshold")]
        public float samplingThreshold = 1f;

        public override void Execute(NavMeshContext context)
        {
            // Gets a random point within 1 radius and times it by moveRadius
            var position = Random.onUnitSphere * this.moveRadius;
            position.y = 0f;

            var navAgent = context.navAgent;
            UnityEngine.AI.NavMeshHit hit;
            // Finds the closest point to the position on the NavMesh
            if (UnityEngine.AI.NavMesh.SamplePosition(position, out hit, this.samplingThreshold, navAgent.areaMask))
            {
                // Sets the destination of the NavAgent, it will then move towards that position
                navAgent.SetDestination(hit.position);
            }
        }
    }
}