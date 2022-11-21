using UnityEngine;
using System.Collections.Generic;

using VAT.Shared.Extensions;
using VAT.Shared.Math;

namespace VAT.Shared.Extensions {
    public static partial class PhysicsExtensions {
        /// <summary>
        /// Returns the closest point on the outside of a sphere.
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="point"></param>
        /// <param name="intersect"></param>
        /// <param name="normal"></param>
        public static void GetSphereIntersect(in Vector3 center, in float radius, in Vector3 point, out Vector3 intersect, out Vector3 normal) {
            var direction = (center - point).normalized;
            if (direction == Vector3.zero)
                direction = Vector3.forward;
            normal = -direction;
            intersect = center - direction * radius;
        }

        /// <summary>
        /// Returns the closest point on the outside of a sphere collider
        /// </summary>
        /// <param name="collider"></param>
        /// <param name="point"></param>
        /// <param name="intersect"></param>
        /// <param name="normal"></param>
        public static void GetSphereIntersect(this SphereCollider collider, in Vector3 point, out Vector3 intersect, out Vector3 normal) => GetSphereIntersect(collider.bounds.center, collider.radius * collider.transform.lossyScale.Maximum(), point, out intersect, out normal);

        /// <summary>
        /// Returns the closest point on the outside of a cylinder.
        /// </summary>
        /// <param name="center">The world center of the cylinder.</param>
        /// <param name="axis">The world axis of the cylinder.</param>
        /// <param name="radius">The local radius of the cylinder.</param>
        /// <param name="height">The local height of the cylinder.</param>
        /// <param name="point">The point to check for.</param>
        /// <param name="initialDirection">The direction the point is shooting a ray.</param>
        /// <param name="intersect">The resulting intersect point.</param>
        /// <param name="normal">The normal of the intersection.</param>
        public static void GetCylinderIntersect(Vector3 center, Vector3 axis, float radius, float height, Vector3 point, Vector3 initialDirection, out Vector3 intersect, out Vector3 normal) {
            var heightDir = axis * height;
            var maxPoint = center + heightDir;
            var minPoint = center - heightDir;
            var lineData = new LineData(maxPoint, minPoint);

            var pointOnLine = lineData.ClosestPointOnLine(point);
            var direction = (pointOnLine - point).normalized;

            direction = Vector3.RotateTowards(direction, initialDirection, 0.79f / Mathf.Max(radius * 5f, 1f), Mathf.Infinity);

            Vector3.OrthoNormalize(ref axis, ref direction);
            var radiusDir = direction * radius;
            intersect = pointOnLine - radiusDir;
            normal = -direction;
        }

    }
}