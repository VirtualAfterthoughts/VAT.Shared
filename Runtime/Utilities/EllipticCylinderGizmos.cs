using UnityEngine;

using VAT.Shared.Data;

namespace VAT.Shared.Utilities
{
    /// <summary>
    /// Functions for drawing <see cref="EllipticCylinder"/> gizmos.
    /// </summary>
    public static class EllipticCylinderGizmos
    {
        /// <summary>
        /// Draws an elliptic cylinder at a given position and axis.
        /// </summary>
        /// <param name="ellipticCylinder"></param>
        /// <param name="position"></param>
        /// <param name="axis"></param>
        public static void DrawEllipticCylinder(EllipticCylinder ellipticCylinder, Vector3 position, Vector3 axis) => DrawEllipticCylinder(ellipticCylinder, 360f, position, axis);

        /// <summary>
        /// Draws an elliptic cylinder with a maximum angle at a given position and axis.
        /// </summary>
        /// <param name="ellipticCylinder"></param>
        /// <param name="angle"></param>
        /// <param name="position"></param>
        /// <param name="axis"></param>
        public static void DrawEllipticCylinder(EllipticCylinder ellipticCylinder, float angle, Vector3 position, Vector3 axis) => DrawEllipticCylinder(ellipticCylinder.Ellipse.Radius, ellipticCylinder.Height, angle, position, axis);

        /// <summary>
        /// Draws an elliptic cylinder with a specified radius and height at a given position and axis.
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="height"></param>
        /// <param name="position"></param>
        /// <param name="axis"></param>
        public static void DrawEllipticCylinder(Vector2 radius, float height, Vector3 position, Vector3 axis) => DrawEllipticCylinder(radius, height, 360f, position, axis);

        /// <summary>
        /// Draws an elliptic cylinder with a specified radius, height, and maximum angle at a given position and axis.
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="height"></param>
        /// <param name="angle"></param>
        /// <param name="position"></param>
        /// <param name="axis"></param>
        public static void DrawEllipticCylinder(Vector2 radius, float height, float angle, Vector3 position, Vector3 axis)
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
