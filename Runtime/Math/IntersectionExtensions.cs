using UnityEngine;

using VAT.Shared.Extensions;

namespace VAT.Shared.Math
{
    /// <summary>
    /// Functions for calculating the intersections of geometry.
    /// </summary>
    public static class Intersections
    {
        /// <summary>
        /// Gets the closest point on a sphere's surface to a given point.
        /// </summary>
        /// <param name="center">The center of the sphere.</param>
        /// <param name="radius">The radius of the sphere.</param>
        /// <param name="point">The point that will be checked.</param>
        /// <param name="closestPoint">The intersection point on the sphere's surface.</param>
        /// <param name="normal">The normal of the sphere at the intersection point.</param>
        public static void GetClosestPointOnSphere(Vector3 center, float radius, Vector3 point, out Vector3 closestPoint, out Vector3 normal)
        {
            var direction = point - center;

            normal = direction.normalized;
            closestPoint = center + normal * radius;
        }

        /// <summary>
        /// Gets the closest point on a SphereCollider's surface to a given point.
        /// </summary>
        /// <param name="collider">The SphereCollider.</param>
        /// <param name="point">The point that will be checked.</param>
        /// <param name="closestPoint">The closest point on the SphereCollider's surface.</param>
        /// <param name="normal">The normal of the SphereCollider at the closest point.</param>
        public static void GetClosestPointOnSphere(this SphereCollider collider, Vector3 point, out Vector3 closestPoint, out Vector3 normal) => GetClosestPointOnSphere(collider.bounds.center, collider.radius * collider.transform.lossyScale.Max(), point, out closestPoint, out normal);
    }
}