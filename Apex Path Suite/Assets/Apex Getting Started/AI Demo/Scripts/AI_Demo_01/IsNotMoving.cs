namespace Apex.GettingStarted.AI
{
    using Apex.AI;

    /// <summary>
    /// A Scorer class which determines the score based on if the Nav Agent is not walking
    /// </summary>
    public sealed class IsNotMoving : ContextualScorerBase<NavMeshContext>
    {
        public override float Score(NavMeshContext context)
        {
            var navAgent = context.navAgent;
            // If the navagent is already walking, return 0, which means that it will not do the action
            if (navAgent.velocity.sqrMagnitude > 0f && navAgent.hasPath)
            {
                return 0f;
            }

            return this.score;
        }
    }
}