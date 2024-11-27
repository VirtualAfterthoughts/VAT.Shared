using UnityEngine;

namespace VAT.Shared.Extensions
{
    public static class CollisionExtensions
    {
        /// <summary>
        /// Calculates the center of all contact points.
        /// </summary>
        /// <param name="collision">The collision to check.</param>
        /// <returns>The contact center.</returns>
        public static Vector3 GetContactCenter(this Collision collision)
        {
            if (collision.contactCount <= 0)
            {
                return Vector3.zero;
            }

            var center = Vector3.zero;

            for (var i = 0; i < collision.contactCount; i++)
            {
                center += collision.GetContact(i).point;
            }

            return center / collision.contactCount;
        }

        /// <summary>
        /// Calculates the average separation of all contact points.
        /// </summary>
        /// <param name="collision">The collision to check.</param>
        /// <returns>The average separation</returns>
        public static float GetAverageSeparation(this Collision collision)
        {
            if (collision.contactCount <= 0)
            {
                return 0f;
            }

            var center = 0f;

            for (var i = 0; i < collision.contactCount; i++)
            {
                center += collision.GetContact(i).separation;
            }

            return center / collision.contactCount;
        }

        /// <summary>
        /// Calculates the average normal of all contact points.
        /// </summary>
        /// <param name="collision">The collision to check.</param>
        /// <returns>The average normal.</returns>
        public static Vector3 GetAverageNormal(this Collision collision)
        {
            if (collision.contactCount <= 0)
            {
                return Vector3.zero;
            }

            var center = Vector3.zero;

            for (var i = 0; i < collision.contactCount; i++)
            {
                center += collision.GetContact(i).normal;
            }

            return center / collision.contactCount;
        }
    }
}