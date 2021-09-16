namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Apex.Serialization;

    public sealed class IsRandomValue : ContextualScorerBase
    {
        [ApexSerialization]
        public float chance = 0.25f;

        public override float Score(IAIContext context)
        {
            if (UnityEngine.Random.value < this.chance)
            {
                return this.score;
            }

            return 0f;
        }
    }
}