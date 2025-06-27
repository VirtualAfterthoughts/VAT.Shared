using UnityEngine;

using VAT.Shared.Data;
using VAT.Shared.Extensions;

namespace VAT.Shared.Math
{
    public static class Intersections
    {
        public static void GetSphereIntersect(in Vector3 center, in float radius, in Vector3 point, out Vector3 intersect, out Vector3 normal)
        {
            var direction = center - point;

            normal = -direction;
            intersect = center - direction * radius;
        }

        public static void GetSphereIntersect(this SphereCollider collider, in Vector3 point, out Vector3 intersect, out Vector3 normal) => GetSphereIntersect(collider.bounds.center, collider.radius * collider.transform.lossyScale.Max(), point, out intersect, out normal);

        public static void GetCylinderIntersect(Vector3 center, Vector3 axis, float radius, float height, Vector3 point, Vector3 initialDirection, out Vector3 intersect, out Vector3 normal)
        {
            var heightDir = axis * height;
            var maxPoint = center + heightDir;
            var minPoint = center - heightDir;
            var line = new Line(maxPoint, minPoint);

            var pointOnLine = line.ClosestPointOnLine(point);
            var direction = (pointOnLine - point).normalized;

            direction = Vector3.RotateTowards(direction, initialDirection, 0.79f / Mathf.Max(radius * 5f, 1f), Mathf.Infinity);

            Vector3.OrthoNormalize(ref axis, ref direction);
            var radiusDir = direction * radius;
            intersect = pointOnLine - radiusDir;
            normal = -direction;
        }
    }
}