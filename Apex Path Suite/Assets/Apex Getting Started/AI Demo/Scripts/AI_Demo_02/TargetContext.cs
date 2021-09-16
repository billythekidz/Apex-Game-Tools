namespace Apex.GettingStarted.AI
{
    using Apex.AI;
    using UnityEngine;

    public sealed class TargetContext : IAIContext
    {
        public TargetContext(Transform transform, GameObject[] targets)
        {
            this.self = transform;
            this.targets = targets;
        }

        public Transform self
        {
            get;
            private set;
        }

        // Since we want the AI to be aware of other GameObjects, we add them to the context
        public GameObject[] targets
        {
            get;
            private set;
        }
    }
}