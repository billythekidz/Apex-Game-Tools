namespace Apex.GettingStarted.AI
{
    using Apex.AI;
    using Serialization;
    using UnityEngine;

    public sealed class EntityScanner : ActionBase<ObservationContext>
    {
        [ApexSerialization(defaultValue = 5f), FriendlyName("Radius", "The radius in which the AI will scan for entities")]
        public float radius = 5f;

        [ApexSerialization, FriendlyName("Layer", "The layer that is scanned against")]
        public LayerMask layer;

        public override void Execute(ObservationContext context)
        {
            // Gets all the GameObjects on the specified layer within the radius
            var hits = Physics.OverlapSphere(context.self.transform.position, this.radius, this.layer);
            for (int i = 0; i < hits.Length; i++)
            {
                var gameObject = hits[i].gameObject;

                // We check if the target is ourselves, we dont want to observe ourselves
                if (ReferenceEquals(gameObject, context.self))
                {
                    continue;
                }

                context.AddOrUpdateObservation(new Observation(gameObject));
            }
        }
    }
}