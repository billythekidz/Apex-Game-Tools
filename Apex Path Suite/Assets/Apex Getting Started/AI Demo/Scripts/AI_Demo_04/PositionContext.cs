namespace Apex.GettingStarted.AI
{
    using System.Collections.Generic;
    using Apex.AI;
    using UnityEngine;

    public sealed class PositionContext : IAIContext
    {
        public PositionContext(GameObject gameObject)
        {
            this.self = gameObject.transform;
            this.positions = new List<Vector3>();
        }

        public Transform self
        {
            get;
            private set;
        }

        // This will contain all the sampled positions
        public IList<Vector3> positions
        {
            get;
            private set;
        }
    }
}