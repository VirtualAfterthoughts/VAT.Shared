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

            EllipseGizmos.DrawEllipse(radius, angle, position - axis * halfHeight, rotation);

            EllipseGizmos.DrawEllipse(radius, angle, position + axis * halfHeight, rotation);

            EllipseGizmos.DrawEllipse(radius, angle, position, rotation);
        }
    }
}
