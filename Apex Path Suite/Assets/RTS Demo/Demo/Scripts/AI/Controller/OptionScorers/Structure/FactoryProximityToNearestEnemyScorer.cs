namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Apex.Serialization;
    using UnityEngine;

    public sealed class FactoryProximityToNearestEnemyScorer : OptionScorerBase<Factory>
    {
        [ApexSerialization(defaultValue = 1f)]
        public float factor = 1f;

        [ApexSerialization]
        public float maxScore = 1000f;

        public override float Score(IAIContext context, Factory factory)
        {
            var c = (ControllerContext)context;
            var enemies = c.controller.enemyObservations;
            var count = enemies.Count;
            if (count == 0)
            {
                return 0f;
            }

            var shortest = float.MaxValue;
            IEntity nearest = null;
            for (int i = 0; i < count; i++)
            {
                var enemyObs = enemies[i];
                var d = (enemyObs.position - factory.position).sqrMagnitude;
                if (d < shortest)
                {
                    shortest = d;
                    nearest = enemyObs.entity;
                }
            }

            if (nearest == null)
            {
                return 0f;
            }

            var distance = (nearest.position - factory.position).magnitude * this.factor;
            return Mathf.Max(0f, this.maxScore - distance);
        }
    }
}