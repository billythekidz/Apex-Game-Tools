namespace Apex.Demo.RTS.AI
{
    using UnityEngine;

    public class Observation
    {
        public Observation(IEntity entity)
        {
            this.entity = entity;
            this.position = entity.position;
            this.collider = entity.transform.GetComponent<Collider>();
            this.timestamp = Time.timeSinceLevelLoad;
        }

        public IEntity entity
        {
            get;
            private set;
        }

        public Collider collider
        {
            get;
            private set;
        }

        public Vector3 position
        {
            get;
            set;
        }

        public float timestamp
        {
            get;
            set;
        }
    }
}