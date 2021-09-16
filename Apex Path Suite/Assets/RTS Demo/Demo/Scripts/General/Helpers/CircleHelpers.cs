namespace Apex.Demo.RTS.AI
{
    using UnityEngine;

    public static class CircleHelpers
    {
        /// <summary>
        /// Gets a position on a circle.
        /// </summary>
        /// <param name="position">The center position of the circle.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="anglePerSpawn">The angle per spawn, controlling the separation between positions on the circle, as well as the maximum amount of positions.</param>
        /// <param name="index">The index.</param>
        /// <returns>A position on the specified circle.</returns>
        public static Vector3 GetPointOnCircle(Vector3 position, float radius, float anglePerSpawn, int index)
        {
            var max = 360f / anglePerSpawn;
            var ang = (index % max) * anglePerSpawn;
            return new Vector3(
                    position.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad),
                    position.y,
                    position.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad));
        }
    }
}