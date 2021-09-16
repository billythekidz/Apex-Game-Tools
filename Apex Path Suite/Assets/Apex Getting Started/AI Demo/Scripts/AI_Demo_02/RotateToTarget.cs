namespace Apex.GettingStarted.AI
{
    using Apex.AI;
    using UnityEngine;

    // Here we need to extend the class "ActionWithOption" in order to evaluate the GameObjects
    public sealed class RotateToTarget : ActionWithOptions<GameObject>
    {
        public override void Execute(IAIContext c)
        {
            var context = (TargetContext)c;

            // We use the method "GetBest" which will evaluate all the GameObjects that we send in and return the one with the highest score
            var best = this.GetBest(context, context.targets);
            if (best == null)
            {
                return;
            }

            context.self.LookAt(best.transform);
        }
    }
}