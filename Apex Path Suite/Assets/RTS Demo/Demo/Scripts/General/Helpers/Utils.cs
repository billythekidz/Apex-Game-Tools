namespace Apex.Demo.RTS.AI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apex.AI;
    using UnityEngine;
    using Utilities;

    /// <summary>
    /// Static helper class with a bunch of utility methods for convenience.
    /// </summary>
    public static class Utils
    {
        private static readonly Collider[] _colliderBuffer = new Collider[100];
        private static readonly RaycastHit[] _hitsBuffer = new RaycastHit[100];
        private static uint _nextPoolId = 0;
        private static GameObject _ground;

        /// <summary>
        /// The next pool ID to give to the next pooled instance. Needed here statically so that all pools use the same ID.
        /// </summary>
        public static uint nextPoolId
        {
            get { return _nextPoolId++; }
        }

        /// <summary>
        /// The global collider buffer used for overlap sphere casts. The same buffer can be reused globally because everything is single-threaded. This value should be cached because accessing it also clears the buffer.
        /// </summary>
        /// <value>
        /// The collider buffer.
        /// </value>
        public static Collider[] colliderBuffer
        {
            get
            {
                // must clear the buffer every time it is requested
                Array.Clear(_colliderBuffer, 0, _colliderBuffer.Length);
                return _colliderBuffer;
            }
        }

        /// <summary>
        /// The global raycast hits buffer used for sphere casts. The same buffer can be reused globally because everything is single-threaded. This value should be cached because accessing it also clears the buffer.
        /// </summary>
        /// <value>
        /// The hits buffer.
        /// </value>
        public static RaycastHit[] hitsBuffer
        {
            get
            {
                // must clear the buffer every time it is requested
                Array.Clear(_hitsBuffer, 0, _hitsBuffer.Length);
                return _hitsBuffer;
            }
        }

        /// <summary>
        /// Gets the highest scoring element from the given list of options, based on the supplied list of scorers.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context">The context.</param>
        /// <param name="options">The options.</param>
        /// <param name="scorers">The scorers.</param>
        /// <returns></returns>
        public static T GetBestFromList<T>(ControllerContext context, IList<T> options, IList<IOptionScorer<T>> scorers)
        {
            var count = options.Count;
            if (count == 0)
            {
                return default(T);
            }

            var best = options[0];
            var scorersCount = scorers.Count;
            if (scorersCount == 0)
            {
                return best;
            }

            var highestScore = 0f;
            for (int i = 0; i < count; i++)
            {
                var score = 0f;
                for (int j = 0; j < scorersCount; j++)
                {
                    var scorer = scorers[j];
                    if (scorer.isDisabled)
                    {
                        continue;
                    }

                    score += scorer.Score(context, options[i]);
                }

                if (score > highestScore)
                {
                    highestScore = score;
                    best = options[i];
                }
            }

            // we only have a "best" if it scored more than 0, otherwise return 'null'
            return highestScore > 0f ? best : default(T);
        }

        /// <summary>
        /// Gets the highest scoring subset list from the given list of options, based on the supplied list of scorers.
        /// The subset will have desiredCount elements, and will populate the provided optionsBuffer.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context">The context.</param>
        /// <param name="options">The options list.</param>
        /// <param name="desiredCount">The desired count of the subset list.</param>
        /// <param name="scorers">The scorers.</param>
        /// <param name="optionsBuffer">The options buffer to populate with the subset list.</param>
        public static void GetBestSubsetFromList<T>(ControllerContext context, IList<T> options, int desiredCount, IList<IOptionScorer<T>> scorers, IList<ScoredOption<T>> optionsBuffer)
        {
            // First score all the options
            var temp = ListBufferPool.GetBuffer<ScoredOption<T>>(options.Count);
            var count = options.Count;
            for (int i = 0; i < count; i++)
            {
                var option = options[i];

                var score = 0f;
                var scount = scorers.Count;
                for (int j = 0; j < scount; j++)
                {
                    var scorer = scorers[j];
                    if (scorer.isDisabled)
                    {
                        continue;
                    }

                    score += scorer.Score(context, option);
                }

                temp.Add(new ScoredOption<T>(option, score));
            }

            // Sort all the scored options and then take the highest scoring ones, which are added to the options buffer.
            optionsBuffer.AddRange(temp.OrderByDescending(t => t.score).Take(desiredCount));
            ListBufferPool.ReturnBuffer(temp);
        }

        /// <summary>
        /// Determines whether the given <see cref="CompareOperator"/> is true.
        /// Always returns true if 'None' is supplied.
        /// </summary>
        /// <param name="op">The compare operation.</param>
        /// <param name="value">The value.</param>
        /// <param name="comparison">The comparison or threshold that the value is compared against.</param>
        /// <returns></returns>
        public static bool IsOperatorTrue(CompareOperator op, float value, float comparison)
        {
            switch (op)
            {
                case CompareOperator.Equals:
                {
                    return value == comparison;
                }

                case CompareOperator.GreaterThan:
                {
                    return value > comparison;
                }

                case CompareOperator.GreaterThanOrEquals:
                {
                    return value >= comparison;
                }

                case CompareOperator.LessThan:
                {
                    return value < comparison;
                }

                case CompareOperator.LessThanOrEquals:
                {
                    return value <= comparison;
                }

                case CompareOperator.NotEquals:
                {
                    return value != comparison;
                }

                case CompareOperator.None:
                {
                    return true;
                }

                default:
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets the total size of the map, based on the 'Terrain' game object's box collider size.
        /// </summary>
        /// <returns></returns>
        public static float GetTotalMapSize()
        {
            if (_ground == null)
            {
                _ground = GameObject.FindGameObjectWithTag("Terrain");
                if (_ground == null)
                {
                    Debug.LogError("GetTotalMapSize() could not find the ground object marked with the 'Terrain' tag");
                    return 0f;
                }
            }

            var collider = _ground.GetComponent<BoxCollider>();
            return _ground.transform.localScale.x * collider.size.x;
        }

        /// <summary>
        /// Gets the supplied position as grounded, by raycasting towards the ground layer and using the hit point with the ground layer.
        /// </summary>
        /// <param name="position">The position to ground.</param>
        /// <returns></returns>
        public static Vector3 GetGroundedPosition(Vector3 position)
        {
            var ray = new Ray(position + (Vector3.up * 10f), Vector3.down);

            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit, 20f, LayersHelper.instance.groundLayer))
            {
                return position;
            }

            return hit.point;
        }

        /// <summary>
        /// Returns -1 when to the left, 1 to the right and 0 for forward/backward.
        /// </summary>
        /// <param name="forward">The forward vector.</param>
        /// <param name="targetDirection">The target direction.</param>
        /// <returns></returns>
        public static float GetAngleDirection(Vector3 forward, Vector3 targetDirection)
        {
            var perp = Vector3.Cross(forward, targetDirection);
            var dir = Vector3.Dot(perp, Vector3.up);

            if (dir > 0f)
            {
                return 1f;
            }
            else if (dir < 0f)
            {
                return -1;
            }

            return 0f;
        }
    }
}