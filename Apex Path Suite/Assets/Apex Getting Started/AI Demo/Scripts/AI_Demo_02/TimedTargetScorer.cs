namespace Apex.GettingStarted.AI
{
    using System;
    using Apex.AI;
    using Apex.Serialization;
    using UnityEngine;

    // Because we are extending "OptionScorerBase" and using GameObject as a type argument, we can then send that GameObject to the scorer and "Evaluate" the GameObject however we'd like
    public sealed class TimedTargetScorer : OptionScorerBase<GameObject>
    {
        [ApexSerialization(defaultValue = 10f), FriendlyName("Score", "The score output for the option that evaluates true")]
        public float score = 10f;

        public override float Score(IAIContext c, GameObject option)
        {
            var context = (TargetContext)c;
            var targets = context.targets;
            var index = Array.IndexOf(targets, option);

            // Here we just compare the time with the index of the gameObject in the "targets"-array, so it will basically loop through the array based on time
            if (Mathf.RoundToInt(Time.time) % targets.Length == index)
            {
                return this.score;
            }

            return 0f;
        }
    }
}