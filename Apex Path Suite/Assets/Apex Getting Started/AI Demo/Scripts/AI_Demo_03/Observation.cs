namespace Apex.GettingStarted.AI
{
    using UnityEngine;

    // This is a wrapper class for everything we want to observe. In here we add all the data that we want to observe, in this case we only want to observe the GameObject
    public class Observation
    {
        public Observation(GameObject gameObject)
        {
            this.gameObject = gameObject;
            this.timestamp = Time.timeSinceLevelLoad;
        }

        public GameObject gameObject
        {
            get;
            private set;
        }

        // We add a timestamp to make us able to check for more up to date data
        public float timestamp
        {
            get;
            set;
        }
    }
}