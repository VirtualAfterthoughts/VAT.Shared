using UnityEngine;

using VAT.Shared.Data;

namespace VAT.Shared.Utilities
{
    public static class EllipseGizmos
    {
        public static void DrawEllipse(Ellipse ellipse, Vector3 position, Quaternion rotation) => DrawEllipse(ellipse, 360f, position, rotation);

        public static void DrawEllipse(Ellipse ellipse, float angle, Vector3 position, Quaternion rotation) => DrawEllipse(ellipse.Radius, angle, position, rotation);

        public static void DrawEllipse(Vector2 radius, Vector3 position, Quaternion rotation) => DrawEllipse(radius, 360f, position, rotation);

        public static void DrawEllipse(Vector2 radius, float angle, Vector3 position, Quaternion rotation)
        {
            var ellipse = new Ellipse() { Radius = radius };
            var points = ellipse.GetLocalPoints(angle);

            for (var i = 0; i < points.Length; i++)
            {
                if (i > 0)
                {
                    Gizmos.DrawLine((rotation * points[i - 1]) + position, (rotation * points[i]) + position);
                }
            }
        }
    }
}
