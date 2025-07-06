using UnityEngine;

using VAT.Shared.Data;

namespace VAT.Shared.Utilities
{
    public static class EllipsoidGizmos
    {
        public static void DrawEllipsoid(Ellipsoid ellipsoid, Vector3 position, Vector3 axis) => DrawEllipsoid(ellipsoid, 360f, position, axis);

        public static void DrawEllipsoid(Ellipsoid ellipsoid, float angle, Vector3 position, Vector3 axis) => DrawEllipsoid(ellipsoid.Ellipse.Radius, ellipsoid.Height, angle, position, axis);

        public static void DrawEllipsoid(Vector2 radius, float height, Vector3 position, Vector3 axis) => DrawEllipsoid(radius, height, 360f, position, axis);

        public static void DrawEllipsoid(Vector2 radius, float height, float angle, Vector3 position, Vector3 axis)
        {
            var rotation = Quaternion.FromToRotation(Vector3.forward, Vector3.up) * Quaternion.LookRotation(axis);

            var halfHeight = height * 0.5f;

            var top = position + axis * halfHeight;
            var bottom = position - axis * halfHeight;

            var ellipse = new Ellipse(radius);

            EllipseGizmos.DrawEllipses(ellipse, new(top, rotation), ellipse, new(bottom, rotation), angle);
        }
    }
}
