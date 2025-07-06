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

        public static void DrawEllipses(Ellipse startEllipse, SimpleTransform startTransform, Ellipse endEllipse, SimpleTransform endTransform) => DrawEllipses(startEllipse, startTransform, endEllipse, endTransform, 360f);

        public static void DrawEllipses(Ellipse startEllipse, SimpleTransform startTransform, Ellipse endEllipse, SimpleTransform endTransform, float angle)
        {
            DrawEllipse(startEllipse, angle, startTransform.Position, startTransform.Rotation);

            DrawEllipse(endEllipse, angle, endTransform.Position, endTransform.Rotation);

            var startEdges = GetEdgeOffsets(startEllipse, angle);

            var endEdges = GetEdgeOffsets(endEllipse, angle);

            for (var i = 0; i < startEdges.Length; i++)
            {
                var startEdge = startTransform.Position + startTransform.Rotation * startEdges[i];
                var endEdge = endTransform.Position + endTransform.Rotation * endEdges[i];

                Gizmos.DrawLine(startEdge, endEdge);
            }
        }

        private static Vector3[] GetEdgeOffsets(Ellipse ellipse, float angle)
        {
            Vector3[] edgeOffsets = new Vector3[5];

            float edgeAngle = angle / (edgeOffsets.Length - 1);

            for (var i = 0; i < edgeOffsets.Length; i++)
            {
                float radians = Mathf.Deg2Rad * edgeAngle * i;

                edgeOffsets[i] = new Vector3(Mathf.Sin(radians) * ellipse.Radius.x, 0f, Mathf.Cos(radians) * ellipse.Radius.y);
            }

            return edgeOffsets;
        }
    }
}
