namespace Apex.Demo.RTS.AI
{
    using System.Collections.Generic;
    using Apex.AI;
    using UnityEngine;

    public class UnitContext : IAIContext
    {
        public UnitContext(IUnit unit)
        {
            this.unit = unit;
            this.sampledPositions = new List<Vector3>(64);
            this.waypoints = new Queue<Vector3>(5);
        }

        public IUnit unit
        {
            get;
            private set;
        }

        public IList<Vector3> sampledPositions
        {
            get;
            private set;
        }

        public Queue<Vector3> waypoints
        {
            get;
            private set;
        }

        public IHasHealth temporaryTarget
        {
            get;
            set;
        }

        public IHasHealth attackTarget
        {
            get;
            set;
        }

        public BuildTarget buildTarget
        {
            get;
            set;
        }

        public IStructure defendTarget
        {
            get;
            set;
        }

        public Vector3? guardTarget
        {
            get;
            set;
        }

        public Vector3? moveTarget
        {
            get;
            set;
        }
    }
}