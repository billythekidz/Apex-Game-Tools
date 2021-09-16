namespace Apex.GettingStarted.AI
{
    using System.Collections.Generic;
    using Apex.AI;
    using UnityEngine;

    public sealed class ObservationContext : IAIContext
    {
        public ObservationContext(GameObject gameObject)
        {
            this.self = gameObject;
            this.observations = new List<Observation>();
        }

        public GameObject self
        {
            get;
            private set;
        }

        public IList<Observation> observations
        {
            get;
            private set;
        }

        public void AddOrUpdateObservation(Observation observation)
        {
            bool existing = false;
            var count = this.observations.Count;
            for (int i = 0; i < count; i++)
            {
                var obs = this.observations[i];

                if (!ReferenceEquals(obs.gameObject, observation.gameObject))
                {
                    // If we already have observed a GameObject, we don't want to re-add it, instead we want to update it's data
                    continue;
                }

                if (obs.timestamp > observation.timestamp)
                {
                    // Only update observations that have a newer timestamp than the current existing observation
                    continue;
                }

                obs.timestamp = observation.timestamp;
                existing = true;
            }

            if (!existing)
            {
                // If the GameObject has never been observed, we add it to the list
                this.observations.Add(observation);
            }
        }
    }
}